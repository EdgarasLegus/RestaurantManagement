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
        Task UpdateCreatedOrderItemsStatuses(int id, List<Dish> dishList);
        Task<OrderItemViewModel> CreateCustomerOrderItem(int orderId, OrderItemCreateModel orderItemCreateEntity);
        Task<OrderItem> GetSelectedOrderItem(int id);
        Task DeleteCustomerOrderItem(int itemId);
        Task PrepareOrderItems(int orderId);
        Task ChangePreparingOrderItemStatus(OrderItem orderItemEntity);
    }
}
