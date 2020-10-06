using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces.Services;
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
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialDishes"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var dishesList = new List<Dish>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string id = line.Split(',').First();
                    string dishName = line.Split(',').ElementAt(1);
                    string isOnMenu = line.Split(',').ElementAt(2);
                    string dishType = line.Split(',').ElementAt(3);

                    var dish = new Dish()
                    {
                        //IsOnMenu = Convert.ToBoolean(isOnMenu),
                        Id = Int32.Parse(id),
                        DishName = dishName,
                        IsOnMenu = _logicHandler.BooleanConverter(isOnMenu),
                        DishType = dishType
                    };
                    if (!dishesList.Any(x => x.DishName == dish.DishName))
                    {
                        dishesList.Add(dish);
                    }
                }
            }
            return dishesList;

        }
    }
}
