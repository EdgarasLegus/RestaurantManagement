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
    public class PersonRoleService : IPersonRoleService
    {
        public List<PersonRole> GetInitialPersonRoles()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["InitialData:InitialPersonRoles"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var roleList = new List<PersonRole>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string id = line.Split(',').First();
                    string roleName = line.Split(',').ElementAt(1);

                    var role = new PersonRole()
                    {
                        Id = Int32.Parse(id),
                        RoleName = roleName
                    };
                    if (!roleList.Any(x => x.RoleName == role.RoleName))
                    {
                        roleList.Add(role);
                    }
                }
            }
            return roleList;

        }
    }
}
