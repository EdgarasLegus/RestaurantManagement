using RestaurantManagement.Data.Repository;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly RestaurantManagementCodeFirstContext _context;

        public UnitOfWork(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
            DishProductRepo = new DishProductRepository(_context);
            DishRepo = new DishRepository(_context);
            OrderItemRepo = new OrderItemRepository(_context);
            OrderRepo = new OrderRepository(_context);
            PersonRoleRepo = new PersonRoleRepository(_context);
            ProductRepo = new ProductRepository(_context);
            RestaurantTableRepo = new RestaurantTableRepository(_context);
            StaffRepo = new StaffRepository(_context);
            UserLogRepo = new UserLogRepository(_context);
        }

        public IDishProductRepo DishProductRepo { get; }
        public IDishRepo DishRepo { get; }
        public IOrderItemRepo OrderItemRepo { get; }
        public IOrderRepo OrderRepo { get; }
        public IPersonRoleRepo PersonRoleRepo { get; }
        public IProductRepo ProductRepo { get; }
        public IRestaurantTableRepo RestaurantTableRepo { get; }
        public IStaffRepo StaffRepo { get; }
        public IUserLogRepo UserLogRepo { get; }

        public Task<int> Commit()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
