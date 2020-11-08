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
        Task AddOrder(Order orderEntity);
        bool CheckOrderUniqueness(string orderName);
        Task<Order> GetOrderById(int id);
        Task<Order> GetOrderByName(string orderName);
        Task<Order> GetOrderWithItems(int id);
        Task EditOrderItems(int id, OrderUpdateModel orderUpdatingEntity);
        Task UpdateOrder(int id, Order orderEntity);
    }
}
