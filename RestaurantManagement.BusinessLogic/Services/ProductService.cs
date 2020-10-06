using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        public List<Product> GetInitialProducts()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialProducts"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var productList = new List<Product>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string id = line.Split(',').First();
                    string productName = line.Split(',').ElementAt(1);
                    string stockAmount = line.Split(',').ElementAt(2);
                    string unitOfMeasure = line.Split(',').ElementAt(3);

                    var product = new Product()
                    {
                        Id = Int32.Parse(id),
                        ProductName = productName,
                        StockAmount = Convert.ToDecimal(stockAmount),
                        UnitOfMeasure = unitOfMeasure
                    };
                    //var matches = myList.Where(p => p.Name == nameToExtract);
                    if (!productList.Any(x => x.ProductName == product.ProductName))
                    {
                        productList.Add(product);
                    }
                }
            }
            return productList;

        }
    }
}
