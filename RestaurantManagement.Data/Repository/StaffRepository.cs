using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Repository
{
    public class StaffRepository : IStaffRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public StaffRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertInitialStaff(List<Staff> staffList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (!_context.Staff.Any())
                {
                    foreach (var staff in staffList)
                    {
                        await _context.Staff.AddAsync(staff);
                    }
                    // Vengti Raw SQL
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

        public async Task<Staff> GetStaffMemberById(int id)
        {
            var staffMember = await _context.Staff.FirstOrDefaultAsync(x => x.Id == id);
            return staffMember;
        }

        public async Task SaveStaffMemberEdition()
        {
            await _context.SaveChangesAsync();
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

        public bool CheckIfStaffMemberIdExists(int memberId)
        {
            var membercheck = _context.Staff.Where(x => x.Id == memberId).Select(x => x.UserName).Count();
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

        // FOR API
        public async Task CreateStaffMember(Staff staffEntity)
        {
            await _context.Staff.AddAsync(staffEntity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveStaffMember(string staffMemberName)
        {
            var memberId = await _context.Staff.Where(x => x.UserName == staffMemberName).Select(x => x.Id).FirstAsync();
            var staffEntity = new Staff
            {
                Id = memberId
            };
           _context.Staff.Remove(staffEntity);
           await _context.SaveChangesAsync();
        }

        // FOR API
        public async Task DeleteStaffMember(Staff staffEntity)
        {
            _context.Staff.Remove(staffEntity);
            await _context.SaveChangesAsync();
        }

        public bool UpdateStaffMember(int updatableStaffMember, StaffUpdateModel updatedStaffMember)
        {
            var staffEntity = _context.Staff.Find(updatableStaffMember);
            //var checkExistingWord = CheckIfStaffMemberExists(staffEntity.UserName);
            if (staffEntity == null)
            {
                return true;
            }
            else
            {
                staffEntity.UserName = updatedStaffMember.UserName;
                staffEntity.UserPassword = updatedStaffMember.UserPassword;
                staffEntity.PersonRoleId = updatedStaffMember.PersonRoleId;
                staffEntity.EndDayOfEmployment = updatedStaffMember.EndDayOfEmployment;
                _context.SaveChanges();
                return false;
            }

            //_context.Staff.Where(x => x.UserName == updatableStaffMember).ToList().ForEach(x => x.UserName = updatedStaffMember.UserName);
            //_context.Staff.Where(x => x.UserPassword == updatableStaffMember).ToList().ForEach(x => x.UserPassword = updatedStaffMember.UserPassword);
            ////var existingWord = _context.Word.Where(x => x.Word1 == newWord.Word1).ToList().First();
            //var checkExistingWord = CheckIfWordExists(newWord.Word1);
            //if (checkExistingWord == true)
            //{
            //    return true;
            //}
            //else
            //{
            //    _context.SaveChanges();
            //    return false;
            //}
            ////_context.SaveChanges();
        }

        // FOR API
        public async Task UpdateStaffMemberInfo(int id, StaffUpdateModel staffEntity)
        {
            var staffMemberEntity = await _context.Staff.FirstOrDefaultAsync(x => x.Id == id);

            staffMemberEntity.UserName = staffEntity.UserName;
            staffMemberEntity.UserPassword = staffEntity.UserPassword;
            staffMemberEntity.PersonRoleId = staffEntity.PersonRoleId;
            staffMemberEntity.EndDayOfEmployment = staffEntity.EndDayOfEmployment;
            await _context.SaveChangesAsync();
        }



    }
}
