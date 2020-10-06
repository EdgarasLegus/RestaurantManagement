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
    public class RestaurantTableRepository : IRestaurantTableRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public RestaurantTableRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialRestaurantTables(List<RestaurantTable> tableColumns)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.RestaurantTable.Any())
                {

                    foreach (var item in tableColumns)
                    {
                        var tableEntity = new RestaurantTable()
                        {
                            Id = item.Id,
                            TableName = item.TableName
                        };
                        await _context.RestaurantTable.AddAsync(tableEntity);
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
