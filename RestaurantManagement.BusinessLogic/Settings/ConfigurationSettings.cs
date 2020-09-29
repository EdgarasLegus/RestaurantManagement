using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Settings
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
    }
}
