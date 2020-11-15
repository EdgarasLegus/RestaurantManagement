using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderViewModel> CreateCustomerOrder(OrderCreateModel orderCreateEntity);
        Task DeleteCustomerOrder(int id);
        Task<Order> GetExistingOrder(int id);

    }
}
