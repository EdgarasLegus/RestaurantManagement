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
            // use IOptions
            var initialProductsFile = Contracts.Settings.ConfigurationSettings.GetInitialProductsFromConfig();
            var fileParts = _logicHandler.FileReader(initialProductsFile);
            var productList = new List<Product>();

            foreach (var product in fileParts.Select(subList => new Product()
            {
                Id = int.Parse(subList[0]),
                ProductName = subList[1],
                StockAmount = Convert.ToDecimal(subList[2]),
                UnitOfMeasure = subList[3]
            }).Where(product => productList.All(x => x.ProductName != product.ProductName)))
            {
                productList.Add(product);
            }
            return productList;

        }
    }
}
