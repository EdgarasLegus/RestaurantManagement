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

        public bool CheckOrderUniqueness(string orderName)
        {
            var orderCheck = _context.Order.Where(x => x.OrderName == orderName).Select(x => x.OrderName).Count();
            if (orderCheck > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public async Task<Order> GetOrderByName(string orderName)
        {
            return await _context.Order.FirstOrDefaultAsync(x => x.OrderName == orderName);
        }

        public async Task<Order> GetOrderWithItems(int id)
        {
            return await _context.Order.Include(x => x.OrderItem).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task EditOrderItems(int id, OrderUpdateModel orderUpdatingEntity)
        {
            var orderEntity = await _context.Order.Include(x => x.OrderItem).FirstOrDefaultAsync(x => x.Id == id);

            
        }

        public async Task UpdateOrder(int id, Order orderEntity)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);

            existingOrder.OrderName = orderEntity.OrderName;
            existingOrder.CreatedDate = orderEntity.CreatedDate;
            existingOrder.ModifiedDate = orderEntity.ModifiedDate;
            existingOrder.OrderStatus = orderEntity.OrderStatus;
            existingOrder.OrderItem = orderEntity.OrderItem;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order orderEntity)
        {
            _context.Order.Remove(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatus(int id, int status)
        {
            var existingOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == id);
            existingOrder.OrderStatus = status;
            await _context.SaveChangesAsync();
        }
    }
}
