using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces;
using RestaurantManagement.Contracts.Interfaces.Repositories;
using RestaurantManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Repository
{
    public class DishRepository : IDishRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public DishRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialDishes(List<Dish> dishColumns)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Dish.Any())
                {
                    foreach (var item in dishColumns)
                    {
                        var dishEntity = new Dish()
                        {
                            Id = item.Id,
                            DishName = item.DishName,
                            IsOnMenu = item.IsOnMenu,
                            DishType = item.DishType
                        };
                        await _context.Dish.AddAsync(dishEntity);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Dish ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Dish OFF");
                    transaction.Commit();
                }
            }
        }
    }
}
