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
        Task<List<Dish>> CalculateDishStock(List<int> dishIdList);
    }
}
