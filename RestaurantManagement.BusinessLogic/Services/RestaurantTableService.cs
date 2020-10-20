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
            var initialRestaurantTablesFile = Contracts.Settings.ConfigurationSettings.GetInitialRestaurantTablesFromConfig();
            var fileParts = _logicHandler.FileReader(initialRestaurantTablesFile);
            var tablesList = new List<RestaurantTable>();

            foreach (List<string> subList in fileParts)
            {
                var tables = new RestaurantTable()
                {
                    Id = Int32.Parse(subList[0]),
                    TableName = subList[1],
                };
                if (!tablesList.Any(x => x.TableName == tables.TableName))
                {
                    tablesList.Add(tables);
                }
            }
            return tablesList;

        }
    }
}
