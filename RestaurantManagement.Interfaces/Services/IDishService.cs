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
        Task<List<Dish>> GetOrderedDishes(List<int> dishIdList);
        Task<Dish> GetSingleDish(int id);
        Task AdjustDishStock(int dishId, int orderedQuantity);
    }
}
