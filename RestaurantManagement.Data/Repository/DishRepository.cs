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
    public class DishRepository : IDishRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public DishRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialDishes(List<Dish> dishList)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            if (!_context.Dish.Any())
            {
                foreach (var dish in dishList)
                {
                    await _context.Dish.AddAsync(dish);
                }
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Dish ON;");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Dish OFF");
                await transaction.CommitAsync();
            }
        }

        public Task<List<Dish>> GetDishes()
        {
            return _context.Dish.ToListAsync();
        }

        // return task to prevent context swithing
        public Task<Dish> GetDishById(int id)
        {
            return _context.Dish.FirstOrDefaultAsync(x => x.Id == id);
        }

        // return task to prevent context swithing
        public async Task<Dish> GetDishWithProducts(int id)
        {
            return await _context.Dish.Include(x => x.DishProduct).FirstOrDefaultAsync(o => o.Id == id);
        }

        // return task to prevent context swithing
        public async Task<List<Dish>> GetSelectedDishes(List<int> dishIdList)
        {
            return await _context.Dish.Where(x => dishIdList.Contains(x.Id)).ToListAsync();
        }

        // remove save changes if using unit of work
        public async Task UpdateDishQuantityInStock(int id, int newQty)
        {
            var dishEntity = await _context.Dish.FirstOrDefaultAsync(x => x.Id == id);
            dishEntity.QuantityInStock = newQty;
            await _context.SaveChangesAsync();
        }
    }
}
