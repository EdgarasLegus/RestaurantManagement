﻿using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IOrderItemRepo
    {
        Task CreateOrderItem(OrderItem orderItemEntity);
        Task<List<int>> GetDishesIdsByOrderId(int id);
        Task UpdateCreatedOrderItemStatus(int orderId, int dishId, int status);
        Task AddOrderItem(OrderItem orderItemEntity);
        Task<OrderItem> GetOrderItemById(int id);
        Task<List<int>> GetOrderItemIdsByOrderId(int id);
        Task UpdatedAddedOrderItemStatus(int id, int status);
        Task DeleteOrderItem(OrderItem orderItemEntity);
        Task<List<OrderItem>> GetOrderItemsByOrderId(int id);
        Task UpdateOrderItemWithPreparedStatus(int currentStatus, int newStatus);
    }
}
