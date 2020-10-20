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
    public class PersonRoleService : IPersonRoleService
    {
        private readonly ILogicHandler _logicHandler;

        public PersonRoleService(ILogicHandler logicHandler)
        {
            _logicHandler = logicHandler;
        }

        public List<PersonRole> GetInitialPersonRoles()
        {
            var initialPersonRolesFile = Contracts.Settings.ConfigurationSettings.GetInitialPersonRolesFromConfig();
            var fileParts = _logicHandler.FileReader(initialPersonRolesFile);
            var rolesList = new List<PersonRole>();

            foreach (List<string> subList in fileParts)
            {
                var role = new PersonRole()
                {
                    Id = Int32.Parse(subList[0]),
                    RoleName = subList[1]
                };
                if (!rolesList.Any(x => x.RoleName == role.RoleName))
                {
                    rolesList.Add(role);
                }
            }
            return rolesList;
        }
    }
}
