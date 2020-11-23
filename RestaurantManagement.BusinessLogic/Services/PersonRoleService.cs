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
            // use IOptions for getting configuration
            var initialPersonRolesFile = Contracts.Settings.ConfigurationSettings.GetInitialPersonRolesFromConfig();
            var fileParts = _logicHandler.FileReader(initialPersonRolesFile);
            var rolesList = new List<PersonRole>();

            foreach (var role in fileParts.Select(subList => new PersonRole()
            {
                Id = int.Parse(subList[0]),
                RoleName = subList[1]
            }).Where(role => rolesList.All(x => x.RoleName != role.RoleName)))
            {
                rolesList.Add(role);
            }
            return rolesList;
        }
    }
}
