using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetOrderItemsByOrderId(int id);
        Task<OrderItem> GetOrderItemById(int id);
        Task<List<int>> GetDishesIdsByOrderId(int id);
        Task AddOrderItem(OrderItem orderItemEntity);
        Task UpdateCreatedOrderItemsStatuses(int orderId);
        Task<OrderItemViewModel> CreateCustomerOrderItem(int orderId, OrderItemCreateModel orderItemCreateEntity);
        Task DeleteCustomerOrderItem(int itemId);
        Task PrepareOrderItems(int orderId);
        Task ReadyToServeOrderItems(int orderId);
        Task UpdateAddedOrderItemStatus(int id, int status);
        Task UpdateCustomerOrderItem(int itemId, OrderItemUpdateModel orderItemUpdateEntity);
        Task UpdateOrderStatusAndDate(int id, int status);
        Task UpdateOrderItemStatus(OrderItem orderItemEntity, int newStatus);
        Task UpdateItemQtyAndFlag(int itemId, OrderItem orderItemEntity);
        Task DeleteOrderItem(OrderItem orderItemEntity);
    }
}
