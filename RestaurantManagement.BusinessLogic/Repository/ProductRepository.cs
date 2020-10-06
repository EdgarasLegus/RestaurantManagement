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
    public class ProductRepository : IProductRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public ProductRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialProducts(List<Product> productColumns)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Product.Any())
                {
                    foreach (var item in productColumns)
                    {
                        var productEntity = new Product()
                        {
                            Id = item.Id,
                            ProductName = item.ProductName,
                            StockAmount = item.StockAmount,
                            UnitOfMeasure = item.UnitOfMeasure
                        };
                        await _context.Product.AddAsync(productEntity);
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
