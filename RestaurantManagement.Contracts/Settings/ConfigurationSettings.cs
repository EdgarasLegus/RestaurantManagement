using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Settings
{
    public class ConfigurationSettings
    {
        public const string InitialData = "InitialData";
        public string InitialStaff { get; set; }
        public string InitialRestaurantTables { get; set; }
        public string InitialProducts { get; set; }
        public string InitialPersonRoles { get; set; }
        public string InitialDishes { get; set; }
        public string InitialDishProducts { get; set; }

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
    }
}
