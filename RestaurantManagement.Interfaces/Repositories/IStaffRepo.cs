using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IStaffRepo
    {
        Task InsertInitialStaff(List<Staff> staffList);
        Task<List<Staff>> GetStaff();
        Task<List<Staff>> SearchStaff(string searchInput);
        Task<List<Staff>> GetStaffMember(string member);

        Task<Staff> GetStaffMemberId(int? id);
        Task SaveStaffMemberEdition();
        bool CheckIfStaffMemberExists(string memberName);
        bool CheckIfStaffMemberIdExists(int memberId);
        Task InsertAdditionalStaffMember(string userName, string userPassword, int personRoleId, DateTime startDay, DateTime endDay);
        Task RemoveStaffMember(string staffMemberName);
        bool UpdateStaffMember(int updatableStaffMember, Staff updatedStaffMember);
    }
}
