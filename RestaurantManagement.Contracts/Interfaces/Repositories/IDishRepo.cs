using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Contracts.Interfaces.Repositories
{
    public interface IDishRepo
    {
        Task InsertInitialDishes(List<Dish> dishColumns);
    }
}
