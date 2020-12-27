using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserLog> _userLogRepo;

        public UserLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userLogRepo = unitOfWork.GetRepository<UserLog>();
        }

        public void InsertUserLog(UserAction userAction, int staffId)
        {
            var userLogEntity = new UserLog
            {
                UserAction = userAction.ToString(),
                ActionTime = DateTime.Now,
                StaffId = staffId
            };
            _userLogRepo.Add(userLogEntity);
            _unitOfWork.Commit();
        }
    }
}
