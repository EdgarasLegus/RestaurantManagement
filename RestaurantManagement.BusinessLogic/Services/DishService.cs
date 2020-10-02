using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RestaurantManagement.BusinessLogic
{
    public class DishService : IDishService
    {
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

            var dishesList = new Dictionary<string, Dish>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string dishName = line.Split(',').First();
                    string isOnMenu = line.Split(',').ElementAt(1);
                    string dishType = line.Split(',').ElementAt(2);

                    var dish = new Dish()
                    {
                        DishName = dishName,
                        IsOnMenu = Convert.ToBoolean(isOnMenu)
                    };
                    if (!dishesList.ContainsValue(dish))
                    {
                        dishesList.Add(dishType, dish);
                    }
                }
            }
            var returnList = dishesList.Select(pair => new Dish()
            {
                DishName = pair.Value.DishName,
                DishType = pair.Key,
                IsOnMenu = pair.Value.IsOnMenu
            }).ToList();
            return returnList;

        }
    }
}
