using Microsoft.Extensions.Configuration;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class StaffService : IStaffService
    {
        public List<Staff> GetInitialStaff()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialStaff"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var staffList = new List<Staff>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string id = line.Split(',').First();
                    string userName = line.Split(',').ElementAt(1);
                    string userPassword = line.Split(',').ElementAt(2);
                    string userRole = line.Split(',').ElementAt(3);
                    string start = line.Split(',').ElementAt(4);
                    string end = line.Split(',').ElementAt(5);

                    var staff = new Staff()
                    {
                        Id = Int32.Parse(id),
                        UserName = userName,
                        UserPassword = userPassword,
                        UserRole = userRole,
                        StartDayOfEmployment = Convert.ToDateTime(start),
                        EndDayOfEmployment = Convert.ToDateTime(end)
                    };
                    if (!staffList.Any(x => x.UserName == staff.UserName))
                    {
                        staffList.Add(staff);
                    }
                }
            }
            return staffList;

        }
    }
}
