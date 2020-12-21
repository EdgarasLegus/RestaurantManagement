using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RestaurantManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data
{
    //public abstract class Repository<T> : IRepository<T> where T : class
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestaurantManagementCodeFirstContext _context;
        private readonly DbSet<T> _entities;

        public Repository(RestaurantManagementCodeFirstContext context)
        {
            //_entities = context.Set<T>();
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);

            return await query.ToListAsync();
        }

        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);

            return query.FirstOrDefaultAsync();
        }

        public async Task Add(T entity)
        {
            //await _entities.AddAsync(entity);
            await _context.AddAsync(entity);
        }
        //public Task<T> Add(T entity)
        //{
        //    return _entities.AddAsync(entity);
        //}

        public void Delete(T entity)
        {
            //_entities.Remove(entity);
            _context.Remove(entity);
        }

        public async Task InsertInitialEntity(List<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Set<T>().Any())
                {
                    foreach (var entity in entityList)
                    {
                        await _context.Set<T>().AddAsync(entity);
                    }
                    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT RestaurantManagementData.dbo.{typeof(T).Name} ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT RestaurantManagementData.dbo.{typeof(T).Name} OFF");
                    transaction.Commit();
                }
            }
        }
    }
}
