using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IDishService
    {
        List<Dish> GetInitialDishes();
        Task<List<Dish>> GetDishes();
        Task<Dish> GetDishById(int id);
        Task<List<Dish>> GetOrderedDishes(List<int> dishIdList);
        Task<Dish> GetDishWithProducts(int id);
        Task AdjustDishStock(int dishId, int orderedQuantity);
    }
}
