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
        Task<List<int>> GetOrderDishesByOrderId(int id);
        Task UpdateOrderDishStatus(int orderId, int dishId, int status);
    }
}
