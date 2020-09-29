using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts;
using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DishProductsService : IDishProductsService
    {
        public List<DishProducts> GetInitialDishProducts()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialDishProducts"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var dishProductList = new Dictionary<string, DishProducts>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string dishId = line.Split(',').First();
                    string productId = line.Split(',').ElementAt(1);
                    string portion = line.Split(',').ElementAt(2);

                    var dishProduct = new DishProducts()
                    {
                        DishId = Int32.Parse(dishId),
                        ProductId = Int32.Parse(productId),
                    };
                    if (!dishProductList.ContainsValue(dishProduct))
                    {
                        dishProductList.Add(portion, dishProduct);
                    }
                }
            }
            var returnList = dishProductList.Select(pair => new DishProducts()
            {
                DishId = pair.Value.DishId,
                ProductId = pair.Value.ProductId,
                Portion = Convert.ToDecimal(pair.Key)
            }).ToList();
            return returnList;

        }
    }
}
