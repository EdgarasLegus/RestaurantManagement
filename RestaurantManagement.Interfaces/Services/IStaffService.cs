using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IStaffService
    {
        List<Staff> GetInitialStaff();
        Task<List<Staff>> GetStaff();
        Task<List<Staff>> SearchStaff(string searchInput);
        // FOR MVC
        Task<List<Staff>> GetStaffMember(string member);
        Task<Staff> GetStaffMemberById(int id);
        // FOR MVC
        Task<bool> CheckIfStaffMemberExists(string userName);
        // FOR MVC
        Task<bool> CheckIfStaffMemberIdExists(int memberId);
        // FOR MVC
        Task InsertAdditionalStaffMember(string userName, string userPassword,
            int personRoleId, DateTime startDay, DateTime endDay);
        //----------------------------------------------------- upper part will be simplified
        // FOR API
        Task<StaffViewModel> CreateStaffMember(StaffCreateModel staffCreateEntity);
        Task DeleteStaffMember(int id);
        Task UpdateStaffMember(int id, StaffUpdateModel staffEntity);
    }
}
