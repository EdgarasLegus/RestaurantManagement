using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DishService : IDishService
    {
        private readonly ILogicHandler _logicHandler;

        public DishService(ILogicHandler logicHandler)
        {
            _logicHandler = logicHandler;
        }

        public List<Dish> GetInitialDishes()
        {
            var initialDishesFile = Contracts.Settings.ConfigurationSettings.GetInitialDishesFromConfig();
            var fileParts = _logicHandler.FileReader(initialDishesFile);
            var dishesList = new List<Dish>();

            foreach (List<string> subList in fileParts)
            {
                var dish = new Dish()
                {
                    Id = Int32.Parse(subList[0]),
                    DishName = subList[1],
                    IsOnMenu = _logicHandler.BooleanConverter(subList[2]),
                    DishType = subList[3]
                };
                if (!dishesList.Any(x => x.DishName == dish.DishName))
                {
                    dishesList.Add(dish);
                }
            }
            return dishesList;
        }
    }
}
