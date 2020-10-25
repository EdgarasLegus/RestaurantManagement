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
    public class RestaurantTableRepository : IRestaurantTableRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public RestaurantTableRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialRestaurantTables(List<RestaurantTable> tablesList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.RestaurantTable.Any())
                {
                    foreach (var table in tablesList)
                    {
                        await _context.RestaurantTable.AddAsync(table);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.RestaurantTable ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.RestaurantTable OFF");
                    transaction.Commit();
                }
            }
        }
    }
}
