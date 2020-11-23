using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Repository
{
    public class OrderRepository : IOrderRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public OrderRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public Task<List<Order>> GetOrders()
        {
            return _context.Order.ToListAsync();
        }

        public async Task AddOrder(Order orderEntity)
        {
            await _context.Order.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public async Task<Order> GetOrderWithItems(int id)
        {
            return await _context.Order.Include(x => x.OrderItem).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateOrder(int id, Order orderEntity)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);

            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.CreatedDate = orderEntity.CreatedDate;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = orderEntity.OrderStatus;
            existingOrder.OrderItem = orderEntity.OrderItem;
            existingOrder.IsPreparing = orderEntity.IsPreparing;
            existingOrder.IsReady = orderEntity.IsReady;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order orderEntity)
        {
            _context.Order.Remove(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAndDate(int id, int status)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            existingOrder.OrderStatus = status;
            existingOrder.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExistingOrder(int id, Order orderEntity, int status)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = status;
            existingOrder.IsPreparing = orderEntity.IsPreparing;
            existingOrder.IsReady = orderEntity.IsReady;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderNameAndStatus(int id, Order orderEntity, int status)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = status;
            await _context.SaveChangesAsync();
        }
    }
}
