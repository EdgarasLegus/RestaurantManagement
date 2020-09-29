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
    public class ProductService : IProductService
    {
        public List<Products> GetInitialProducts()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialProducts"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var productList = new Dictionary<string, Products>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string productName = line.Split(',').First();
                    string stockAmount = line.Split(',').ElementAt(1);
                    string unitOfMeasure = line.Split(',').ElementAt(2);

                    var product = new Products()
                    {
                        ProductName = productName,
                        StockAmount = Convert.ToDecimal(stockAmount)
                    };
                    if (!productList.ContainsValue(product))
                    {
                        productList.Add(unitOfMeasure, product);
                    }
                }
            }
            var returnList = productList.Select(pair => new Products()
            {
                ProductName = pair.Value.ProductName,
                StockAmount = pair.Value.StockAmount,
                UnitOfMeasure = pair.Key
            }).ToList();
            return returnList;

        }
    }
}
