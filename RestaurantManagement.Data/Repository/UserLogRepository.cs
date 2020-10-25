using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Data.Repository
{
    public class UserLogRepository : IUserLogRepo
    {
        private readonly RestaurantManagementCodeFirstContext _context;

        public UserLogRepository(RestaurantManagementCodeFirstContext context)
        {
            _context = context;
        }

        public void InsertUserLog(UserAction userAction, int staffId)
        {
            //var ip = _efLogic.GetIP();
            var userLogEntity = new UserLog
            {
                UserAction = userAction.ToString(),
                ActionTime = DateTime.Now,
                StaffId = staffId
            };
            _context.UserLog.Add(userLogEntity);
            _context.SaveChanges();
        }
    }
}
