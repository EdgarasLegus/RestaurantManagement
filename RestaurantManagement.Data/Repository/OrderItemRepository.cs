using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<int>> GetOrderDishesByOrderId(int id)
        {
            return await _context.OrderItem.Where(x => x.OrderId == id).Select(x => x.DishId).ToListAsync();
        }

        public async Task UpdateOrderDishStatus(int orderId, int dishId, int status)
        {
            var existingOrderItemsList = await _context.OrderItem.Where(x => x.OrderId == orderId).ToListAsync();
            var existingOrderItem = existingOrderItemsList.FirstOrDefault(x => x.DishId == dishId);

            existingOrderItem.OrderItemStatus = status;
            await _context.SaveChangesAsync();
        }
    }
}
