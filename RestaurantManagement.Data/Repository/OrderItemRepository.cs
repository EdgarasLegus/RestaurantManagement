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
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public OrderItemRepository(RestaurantManagementCodeFirstContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateOrderItem(OrderItem orderItemEntity)
        {
            await _context.OrderItem.AddAsync(orderItemEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetDishesIdsByOrderId(int id)
        {
            return await _context.OrderItem.Where(x => x.OrderId == id).Select(x => x.DishId).ToListAsync();
        }

        public async Task AddOrderItem(OrderItem orderItemEntity)
        {
            await _context.OrderItem.AddAsync(orderItemEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderItem> GetOrderItemById(int id)
        {
            return await _context.OrderItem.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<int>> GetOrderItemIdsByOrderId(int id)
        {
            return await _context.OrderItem.Where(x => x.OrderId == id).Select(x => x.Id).ToListAsync();
        }

        public async Task UpdatedAddedOrderItemStatus(int id , int status)
        {
            var existingOrderItem = await _context.OrderItem.FirstOrDefaultAsync(x => x.Id == id);
            existingOrderItem.OrderItemStatus = status;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderItem(OrderItem orderItemEntity)
        {
            _context.OrderItem.Remove(orderItemEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int id)
        {
            return await _context.OrderItem.Where(x => x.OrderId == id).ToListAsync();
        }

        public async Task UpdateOrderItemStatus(OrderItem orderItemEntity, int newStatus)
        {
            var existingOrderItem = await _context.OrderItem.Where(x => x.Id == orderItemEntity.Id).FirstOrDefaultAsync();
            existingOrderItem.OrderItemStatus = newStatus;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemQtyAndFlag(int itemId, OrderItem orderItemEntity)
        {
            var existingOrderItem = await _context.OrderItem.Where(x => x.Id == itemId).FirstOrDefaultAsync();
            existingOrderItem.Quantity = orderItemEntity.Quantity;
            existingOrderItem.IsServed = orderItemEntity.IsServed;
            await _context.SaveChangesAsync();
        }
    }
}
