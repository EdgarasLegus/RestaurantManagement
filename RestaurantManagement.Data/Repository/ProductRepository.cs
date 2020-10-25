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
    public class ProductRepository : IProductRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public ProductRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialProducts(List<Product> productList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Product.Any())
                {
                    foreach (var product in productList)
                    {
                        await _context.Product.AddAsync(product);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Product ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Product OFF");
                    transaction.Commit();
                }
            }
        }
    }
}
