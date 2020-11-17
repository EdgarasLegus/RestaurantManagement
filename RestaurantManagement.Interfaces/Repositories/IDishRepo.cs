using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IDishRepo
    {
        Task InsertInitialDishes(List<Dish> dishList);
        Task<List<Dish>> GetDishes();
        Task<Dish> GetDishById(int id);
        Task<Dish> GetDishWithProducts(int id);
        Task<List<Dish>> GetSelectedDishes(List<int> dishIdList);
        Task UpdateDishQuantityInStock(int id, int newQty);
    }
}
