using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DishService : IDishService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly IDishRepo _dishRepo;

        public DishService(ILogicHandler logicHandler, IDishRepo dishRepo)
        {
            _logicHandler = logicHandler;
            _dishRepo = dishRepo;
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
                    DishType = subList[3],
                    QuantityInStock = Int32.Parse(subList[4])
                };
                if (dishesList.All(x => x.DishName != dish.DishName))
                {
                    dishesList.Add(dish);
                }
            }
            return dishesList;
        }

        public async Task<List<Dish>> GetOrderedDishes(List<int> dishIdList)
        {
            return await _dishRepo.GetSelectedDishes(dishIdList);
        }

        public async Task<Dish> GetSingleDish(int id)
        {
            return await _dishRepo.GetDishById(id);
        }

        public async Task AdjustDishStock(int dishId, int orderedQuantity)
        {
            var dish = await GetSingleDish(dishId);
            var newQty = dish.QuantityInStock - orderedQuantity;
            await UpdateDishStock(dishId, newQty);
        }

        private async Task UpdateDishStock(int id, int newQty)
        {
            await _dishRepo.UpdateDishQuantityInStock(id, newQty);
        }

    }
}
