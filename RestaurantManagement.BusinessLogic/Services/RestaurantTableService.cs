using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
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

        public RestaurantTableService(ILogicHandler logicHandler)
        {
            _logicHandler = logicHandler;
        }

        public List<RestaurantTable> GetInitialRestaurantTables()
        {
            // use IOptions
            var initialRestaurantTablesFile = Contracts.Settings.ConfigurationSettings.GetInitialRestaurantTablesFromConfig();
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
