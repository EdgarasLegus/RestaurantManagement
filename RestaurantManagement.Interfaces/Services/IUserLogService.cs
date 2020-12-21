using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IUserLogService
    {
        void InsertUserLog(UserAction userAction, int staffId);
    }
}
