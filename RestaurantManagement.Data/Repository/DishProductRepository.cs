using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Repository
{
    public class DishProductRepository : Repository<DishProduct>, IDishProductRepo 
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public DishProductRepository(RestaurantManagementCodeFirstContext context) :base(context)
        {
            _context = context;
        }

        public async Task InsertInitialDishProducts(List<DishProduct> dishProductList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.DishProduct.Any())
                {
                    foreach (var dishProduct in dishProductList)
                    {
                        await _context.DishProduct.AddAsync(dishProduct);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.DishProduct ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.DishProduct OFF");
                    transaction.Commit();
                }
            }
        }


    }
}
