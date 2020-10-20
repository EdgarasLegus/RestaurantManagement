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

            var initialStuffFile = Contracts.Settings.ConfigurationSettings.GetInitialStaffFromConfig();
            var fileParts = _logicHandler.FileReader(initialStuffFile);
            var staffList = new List<Staff>();

            foreach (List<string> subList in fileParts)
            {
                var staff = new Staff()
                {
                    Id = Int32.Parse(subList[0]),
                    UserName = subList[1],
                    UserPassword = subList[2],
                    PersonRoleId = Int32.Parse(subList[3]),
                    StartDayOfEmployment = Convert.ToDateTime(subList[4]),
                    EndDayOfEmployment = Convert.ToDateTime(subList[5])
                };
                if (!staffList.Any(x => x.UserName == staff.UserName))
                {
                    staffList.Add(staff);
                }
            }
            return staffList;

        }
    }
}
