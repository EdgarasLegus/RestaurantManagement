using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class RestaurantTableService : IRestaurantTablesService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;

        public RestaurantTableService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
        }

        public List<RestaurantTable> GetInitialRestaurantTables()
        {
            var initialRestaurantTablesFile = _options.InitialRestaurantTables;
            var fileParts = _logicHandler.FileReader(initialRestaurantTablesFile);
            var tablesList = new List<RestaurantTable>();

            foreach (var tables in fileParts.Select(subList => new RestaurantTable()
            {
                Id = int.Parse(subList[0]),
                TableName = subList[1],
            }).Where(tables => tablesList.All(x => x.TableName != tables.TableName)))
            {
                tablesList.Add(tables);
            }
            return tablesList;
        }
    }
}
