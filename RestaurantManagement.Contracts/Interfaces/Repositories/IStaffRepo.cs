using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Contracts.Interfaces.Repositories
{
    public interface IStaffRepo
    {
        Task InsertInitialStaff(List<Staff> staffColumns);
        Task<List<Staff>> GetStaff();
        Task<List<Staff>> SearchStaff(string searchInput);
        Task<List<Staff>> GetStaffMember(string member);
        bool CheckIfStaffMemberExists(string memberName);
        Task InsertAdditionalStaffMember(string userName, string userPassword, string userRole, DateTime startDay, DateTime endDay);
    }
}
