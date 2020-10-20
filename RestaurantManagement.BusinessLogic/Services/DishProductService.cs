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
    public class DishProductService : IDishProductService
    {
        private readonly ILogicHandler _logicHandler;

        public DishProductService(ILogicHandler logicHandler)
        {
            _logicHandler = logicHandler;
        }

        public List<DishProduct> GetInitialDishProducts()
        {
            var initialDishProductsFile = Contracts.Settings.ConfigurationSettings.GetInitialDishProductsFromConfig();
            var fileParts = _logicHandler.FileReader(initialDishProductsFile);
            var dishProductList = new List<DishProduct>();

            foreach (List<string> subList in fileParts)
            {
                var dishProduct = new DishProduct()
                {
                    Id = Int32.Parse(subList[0]),
                    DishId = Int32.Parse(subList[1]),
                    ProductId = Int32.Parse(subList[2]),
                    Portion = Convert.ToDecimal(subList[3])
                };
                if (!dishProductList.Equals(dishProduct))
                {
                    dishProductList.Add(dishProduct);
                }
            }
            return dishProductList;
        }
    }
}
