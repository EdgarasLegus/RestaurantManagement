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
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();

            DishProductRepo = new DishProductRepository(_context);
            DishRepo = new DishRepository(_context);
        }

        public IDishProductRepo DishProductRepo { get; }
        public IDishRepo DishRepo { get; }

        public Task<int> Commit()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T: class
        {
            var targetType = typeof(T);
            if (!_repositories.ContainsKey(targetType))
            {
                _repositories[targetType] = new Repository<T>(this._context);
            }
            return (IRepository<T>)this._repositories[targetType];
        }

    }
}
