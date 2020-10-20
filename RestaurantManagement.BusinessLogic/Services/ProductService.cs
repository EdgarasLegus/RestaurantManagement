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
    public class ProductService : IProductService
    {
        private readonly ILogicHandler _logicHandler;

        public ProductService(ILogicHandler logicHandler)
        {
            _logicHandler = logicHandler;
        }

        public List<Product> GetInitialProducts()
        {
            var initialProductsFile = Contracts.Settings.ConfigurationSettings.GetInitialProductsFromConfig();
            var fileParts = _logicHandler.FileReader(initialProductsFile);
            var productList = new List<Product>();

            foreach (List<string> subList in fileParts)
            {
                var product = new Product()
                {
                    Id = Int32.Parse(subList[0]),
                    ProductName = subList[1],
                    StockAmount = Convert.ToDecimal(subList[2]),
                    UnitOfMeasure = subList[3]
                };
                if (!productList.Any(x => x.ProductName == product.ProductName))
                {
                    productList.Add(product);
                }
            }
            return productList;

        }
    }
}
