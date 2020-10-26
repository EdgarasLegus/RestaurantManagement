using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task CreateOrder(Order orderEntity)
        {
            //orderEntity.OrderItemModel.Add(new OrderItemModel()
            //{
            //    OrderId = orderEntity.Or,
            //    DishId

            //});
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
    }
}
