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
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Dish.Any())
                {
                    foreach (var dish in dishList)
                    {
                        await _context.Dish.AddAsync(dish);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Dish ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Dish OFF");
                    transaction.Commit();
                }
            }
        }

        public Task<List<Dish>> GetDishes()
        {
            return _context.Dish.ToListAsync();
        }

        public async Task<Dish> GetDishById(int id)
        {
            var dish = await _context.Dish.FirstOrDefaultAsync(x => x.Id == id);
            return dish;
        }
        public async Task<Dish> GetDishWithProducts(int id)
        {
            return await _context.Dish.Include(x => x.DishProduct).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Dish>> GetSelectedDishes(List<int> dishIdList)
        {
            return await _context.Dish.Where(x => dishIdList.Contains(x.Id)).ToListAsync();
        }
    }
}
