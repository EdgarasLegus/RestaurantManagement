using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Settings
{
    public class ConfigurationSettings
    {
        private static readonly IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(@"./appsettings.json")
                .Build();

        public static string GetConnectionStringDBFirst()
        {
            var connectionString = configuration["ConnectionProperties:ConnectionStringDBFirst"];
            return connectionString;
        }

        public static string GetConnectionStringCodeFirst()
        {
            var connectionString = configuration["ConnectionProperties:ConnectionStringCodeFirst"];
            return connectionString;
        }

        public static string GetInitialStaffFromConfig()
        {
            var initialStaffFile = configuration["InitialData:InitialStaff"];
            return initialStaffFile;
        }

        public static string GetInitialRestaurantTablesFromConfig()
        {
            var initialRestaurantTablesFile = configuration["InitialData:InitialRestaurantTables"];
            return initialRestaurantTablesFile;
        }

        public static string GetInitialProductsFromConfig()
        {
            var initialProductsFile = configuration["InitialData:InitialProducts"];
            return initialProductsFile;
        }

        public static string GetInitialPersonRolesFromConfig()
        {
            var initialPersonRolesFile = configuration["InitialData:InitialPersonRoles"];
            return initialPersonRolesFile;
        }

        public static string GetInitialDishesFromConfig()
        {
            var initialDishesFile = configuration["InitialData:InitialDishes"];
            return initialDishesFile;
        }

        public static string GetInitialDishProductsFromConfig()
        {
            var initialDishProductsFile = configuration["InitialData:InitialDishProducts"];
            return initialDishProductsFile;
        }
    }
}
