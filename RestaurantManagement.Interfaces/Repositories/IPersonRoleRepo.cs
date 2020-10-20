using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IPersonRoleRepo
    {
        Task InsertInitialPersonRoles(List<PersonRole> roleList);
        List<PersonRole> GetRoles();
    }
}
