﻿using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderById(int id);
        Task<Order> GetOrderWithItems(int id);
        Task AddOrder(Order orderEntity);
        Task<Order> Create(OrderCreateModel orderCreateEntity);
        Task<OrderViewModel> CreateCustomerOrder(OrderCreateModel orderCreateEntity);
        Task DeleteCustomerOrder(int id);
        Task UpdateCreatedOrder(int id, Order orderEntity);
        Task UpdateOrderStatusAndDate(int id, int status);
        Task UpdateExistingOrder(int id, Order orderEntity, int status);
        Task UpdateOrderNameAndStatus(int id, Order orderEntity, int status);
        Task UpdateCustomerOrder(OrderUpdateModel orderUpdateEntity, int orderId);
        Task<bool> OrderHasSomeItemsWithZeroStock(Order orderEntity);
        Task<bool> OrderHasAllItemsWithZeroStock(Order orderEntity);
        Task<Order> PartiallyDecline(Order orderEntity);
        Task<Order> Decline(Order orderEntity);

    }
}
