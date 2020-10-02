using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class RestaurantTableService : IRestaurantTablesService
    {
        public List<RestaurantTable> GetInitialRestaurantTables()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialRestaurantTables"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var tablesList = new List<RestaurantTable>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string tableName = line;
                    var tables = new RestaurantTable()
                    {
                        TableName = tableName,
                    };
                    if (!tablesList.Contains(tables))
                    {
                        tablesList.Add(tables);
                    }
                }
            }
            return tablesList;

        }
    }
}
