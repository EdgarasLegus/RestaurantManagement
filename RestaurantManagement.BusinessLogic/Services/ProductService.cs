using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;

        public ProductService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
        }

        public List<Product> GetInitialProducts()
        {
            var initialProductsFile = _options.InitialProducts;
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
