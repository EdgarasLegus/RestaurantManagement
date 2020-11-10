using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IOrderItemService
    {
        Task UpdateOrderItemsStatuses(int id, List<Dish> dishList);
    }
}
