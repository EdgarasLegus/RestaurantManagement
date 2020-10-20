using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IUserLogRepo
    {
        void InsertUserLog(UserAction userAction, int staffId);
    }
}
