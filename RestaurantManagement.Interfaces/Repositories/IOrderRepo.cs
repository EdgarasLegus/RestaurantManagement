using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetOrders();
        Task CreateOrder(Order orderEntity);
        bool CheckOrderUniqueness(string orderName);
        Task<Order> GetOrderById(int id);
        //Task<Order> GetOrderWithItems(int id);
    }
}
