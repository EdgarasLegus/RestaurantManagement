using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Repository
{
    public class OrderItemRepository : IOrderItemRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public OrderItemRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task CreateOrderItem(OrderItem orderItemEntity)
        {
            await _context.OrderItem.AddAsync(orderItemEntity);
            await _context.SaveChangesAsync();
        }
    }
}
