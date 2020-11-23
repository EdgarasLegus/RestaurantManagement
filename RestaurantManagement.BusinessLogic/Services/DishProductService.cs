using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Int32;

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
            // use IOptions<T> instead for getting configurations. this way the code will not be testable
            var initialDishProductsFile = Contracts.Settings.ConfigurationSettings.GetInitialDishProductsFromConfig();
            var fileParts = _logicHandler.FileReader(initialDishProductsFile);
            var dishProductList = new List<DishProduct>();

            // can be converted to linq
            foreach (var subList in fileParts)
            {
                var dishProduct = new DishProduct
                {
                    Id = Parse(subList[0]),
                    DishId = Parse(subList[1]),
                    ProductId = Parse(subList[2]),
                    Portion = Convert.ToDecimal(subList[3])
                };
                // equals will not work here. beccause comparing two different object types List<DoshProducs> equals DishProduct will always be false
                if (!dishProductList.Equals(dishProduct))
                {
                    dishProductList.Add(dishProduct);
                }
            }
            return dishProductList;
        }
    }
}
