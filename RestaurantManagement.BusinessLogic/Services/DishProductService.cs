using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using static System.Int32;
using System.IO;
using System.Linq;
using System.Text;
using RestaurantManagement.Contracts.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DishProductService : IDishProductService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;

        public DishProductService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
        }

        public List<DishProduct> GetInitialDishProducts()
        {
            var initialDishProductsFile = _options.InitialDishProducts;
            var fileParts = _logicHandler.FileReader(initialDishProductsFile);
            var dishProductList = new List<DishProduct>();

            foreach (var subList in fileParts)
            {
                var dishProduct = new DishProduct
                {
                    Id = Parse(subList[0]),
                    DishId = Parse(subList[1]),
                    ProductId = Parse(subList[2]),
                    Portion = Convert.ToDecimal(subList[3])
                };
                if (!IsDuplicate(dishProductList, dishProduct))
                {
                    dishProductList.Add(dishProduct);
                }
            }
            return dishProductList;
        }

        private bool IsDuplicate(List<DishProduct> dishProductList, DishProduct dishProduct)
        {
            return dishProductList.Any(x => x.DishId == dishProduct.DishId &&
            x.ProductId == dishProduct.ProductId);
        }
    }
}
