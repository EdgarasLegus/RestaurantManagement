using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IPersonRoleService
    {
        List<PersonRole> GetInitialPersonRoles();
        Task<List<PersonRole>> GetRoles();
    }
}
