using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IOrderItemRepo : IRepository<OrderItem>
    {
        Task CreateOrderItem(OrderItem orderItemEntity);
        Task<List<int>> GetDishesIdsByOrderId(int id);
        Task AddOrderItem(OrderItem orderItemEntity);
        Task<OrderItem> GetOrderItemById(int id);
        Task<List<int>> GetOrderItemIdsByOrderId(int id);
        Task UpdatedAddedOrderItemStatus(int id, int status);
        Task DeleteOrderItem(OrderItem orderItemEntity);
        Task<List<OrderItem>> GetOrderItemsByOrderId(int id);
        Task UpdateOrderItemStatus(OrderItem orderItemEntity, int newStatus);
        Task UpdateItemQtyAndFlag(int itemId, OrderItem orderItemEntity);
    }
}
