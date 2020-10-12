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
    public class StaffRepository : IStaffRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public StaffRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialStaff(List<Staff> staffColumns)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Staff.Any())
                {
                    foreach (var item in staffColumns)
                    {
                        var staffEntity = new Staff()
                        {
                            Id = item.Id,
                            UserName = item.UserName,
                            UserPassword = item.UserPassword,
                            PersonRoleId = item.PersonRoleId,
                            StartDayOfEmployment = item.StartDayOfEmployment,
                            EndDayOfEmployment = item.EndDayOfEmployment
                        };
                        await _context.Staff.AddAsync(staffEntity);
                    }
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Staff ON;");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT RestaurantManagementData.dbo.Staff OFF");
                    transaction.Commit();
                }
            }
        }

        public Task<List<Staff>> GetStaff()
        {
            return _context.Staff.ToListAsync();
        }

        public Task<List<Staff>> SearchStaff(string searchInput)
        {
            var staffList = _context.Staff.Where(x => x.UserName.Contains(searchInput)).ToListAsync();
            return staffList;
        }

        public Task<List<Staff>> GetStaffMember(string member)
        {
            var staffMember = _context.Staff.Where(x => x.UserName == member).ToListAsync();
            return staffMember;
        }

        public bool CheckIfStaffMemberExists(string memberName)
        {
            var membercheck = _context.Staff.Where(x => x.UserName == memberName).Select(x => x.UserName).Count();
            if (membercheck > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task InsertAdditionalStaffMember(string userName, string userPassword, int personRoleId, DateTime startDay, DateTime endDay)
        {
            var additionalStaffMember = new Staff
            {
                UserName = userName,
                UserPassword = userPassword,
                PersonRoleId = _context.PersonRole.Where(x => x.Id == personRoleId).Select(x => x.Id).First(),
                StartDayOfEmployment = startDay,
                EndDayOfEmployment = endDay

            };
            await _context.Staff.AddAsync(additionalStaffMember);
            await _context.SaveChangesAsync();
        }

        public void RemoveStaffMember(string staffMemberName)
        {
            var memberId = _context.Staff.Where(x => x.UserName == staffMemberName).Select(x => x.Id).First();
            var staffEntity = new Staff
            {
                Id = memberId
            };
            _context.Staff.Remove(staffEntity);
            _context.SaveChangesAsync();
        }

    }
}
