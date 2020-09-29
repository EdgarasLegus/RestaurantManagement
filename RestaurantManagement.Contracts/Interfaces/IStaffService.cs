using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Interfaces
{
    public interface IStaffService
    {
        List<Staff> GetInitialStaff();
    }
}
