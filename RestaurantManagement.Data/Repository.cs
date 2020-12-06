using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestaurantManagementCodeFirstContext _context;
        private readonly DbSet<T> _entities;

        public Repository(RestaurantManagementCodeFirstContext context)
        {
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }
    }
}
