using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class StaffService : IStaffService
    {
        private readonly ILogicHandler _logicHandler;

        public StaffService(ILogicHandler logicHandler)
        {
            _logicHandler = logicHandler;
        }

        public List<Staff> GetInitialStaff()
        {
            // use IOptions
            var initialStuffFile = Contracts.Settings.ConfigurationSettings.GetInitialStaffFromConfig();
            var fileParts = _logicHandler.FileReader(initialStuffFile);
            var staffList = new List<Staff>();

            foreach (var staff in fileParts.Select(subList => new Staff()
            {
                Id = int.Parse(subList[0]),
                UserName = subList[1],
                UserPassword = subList[2],
                PersonRoleId = int.Parse(subList[3]),
                StartDayOfEmployment = Convert.ToDateTime(subList[4]),
                EndDayOfEmployment = Convert.ToDateTime(subList[5])
            }).Where(staff => staffList.All(x => x.UserName != staff.UserName)))
            {
                staffList.Add(staff);
            }
            return staffList;

        }
    }
}
