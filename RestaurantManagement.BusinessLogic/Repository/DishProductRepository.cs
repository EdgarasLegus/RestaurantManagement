using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces.Repositories;
using RestaurantManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Repository
{
    public class DishProductRepository : IDishProductRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public DishProductRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialDishProducts(List<DishProduct> dishProductColumns)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.DishProduct.Any())
                {
                    foreach (var item in dishProductColumns)
                    {
                        var dishProductEntity = new DishProduct()
                        {
                            Id = item.Id,
                            DishId = item.DishId,
                            ProductId = item.ProductId,
                            Portion = item.Portion
                        };
                        await _context.DishProduct.AddAsync(dishProductEntity);
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
