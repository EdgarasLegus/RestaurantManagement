using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DishProductService : IDishProductService
    {
        public List<DishProduct> GetInitialDishProducts()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialDishProducts"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }
            var dishProductList = new List<DishProduct>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string id = line.Split(',').First();
                    string dishId = line.Split(',').ElementAt(1);
                    string productId = line.Split(',').ElementAt(2);
                    string portion = line.Split(',').ElementAt(3);

                    var dishProduct = new DishProduct()
                    {
                        Id = Int32.Parse(id),
                        DishId = Int32.Parse(dishId),
                        ProductId = Int32.Parse(productId),
                        Portion = Convert.ToDecimal(portion)
                    };
                    if (!dishProductList.Equals(dishProduct))
                    {
                        dishProductList.Add(dishProduct);
                    }
                }
            }
            return dishProductList;

        }
    }
}
