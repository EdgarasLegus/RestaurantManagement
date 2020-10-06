using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Interfaces.Services
{
    public interface IStaffService
    {
        List<Staff> GetInitialStaff();
    }
}
