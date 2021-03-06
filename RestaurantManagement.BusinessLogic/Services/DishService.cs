﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class DishService : IDishService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Dish> _dishRepo;

        public DishService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options,
            IUnitOfWork unitOfWork)
        {
            _logicHandler = logicHandler;
            _options = options.Value;

            _unitOfWork = unitOfWork;
            _dishRepo = _unitOfWork.GetRepository<Dish>();
        }

        public List<Dish> GetInitialDishes()
        {
            var initialDishesFile = _options.InitialDishes;
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

        public Task<List<Dish>> GetDishes()
        {
            return _dishRepo.Get();
        }

        public async Task<Dish> GetDishById(int id)
        {
            return await _dishRepo.GetFirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Dish>> GetOrderedDishes(List<int> dishIdList)
        {
            return await _dishRepo.Get(x => dishIdList.Contains(x.Id));
        }

        public async Task<Dish> GetDishWithProducts(int id)
        {
            return await _dishRepo.GetFirstOrDefault(o => o.Id == id, x => x.Include(x => x.DishProduct));
        }

        public async Task AdjustDishStock(int dishId, int orderedQuantity)
        {
            var dish = await GetDishById(dishId);
            var newQty = dish.QuantityInStock - orderedQuantity;
            await UpdateDishStock(dishId, newQty);
        }

        private async Task UpdateDishStock(int id, int newQty)
        {
            var dishEntity = await _dishRepo.GetFirstOrDefault(x => x.Id == id);
            dishEntity.QuantityInStock = newQty;
            await _unitOfWork.Commit();
        }

    }
}
