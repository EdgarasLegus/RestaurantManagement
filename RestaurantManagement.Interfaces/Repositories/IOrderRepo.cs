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
        Task<Order> GetOrderById(int id);
        Task<Order> GetOrderWithItems(int id);
        Task UpdateOrder(int id, Order orderEntity);
        Task DeleteOrder(Order orderEntity);
        Task UpdateOrderStatusAndDate(int id, int status);
        Task UpdateExistingOrder(int id, Order orderEntity, int status);
        Task UpdateOrderNameAndStatus(int id, Order orderEntity, int status);
    }
}
