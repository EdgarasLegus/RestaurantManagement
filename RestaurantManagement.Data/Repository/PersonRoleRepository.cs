using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data
{
    public class PersonRoleRepository : IPersonRoleRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public PersonRoleRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialPersonRoles(List<PersonRole> roleList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.PersonRole.Any())
                {
                    foreach (var role in roleList)
                    {
                        await _context.PersonRole.AddAsync(role);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.PersonRole ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.PersonRole OFF");
                    transaction.Commit();
                }
            }
        }

        public List<PersonRole> GetRoles()
        {
            return _context.PersonRole.ToList();
        }
    }
}
