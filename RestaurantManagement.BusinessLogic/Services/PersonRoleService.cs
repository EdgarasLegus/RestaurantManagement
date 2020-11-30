using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
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
        private readonly ConfigurationSettings _options;

        public PersonRoleService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
        }

        public List<PersonRole> GetInitialPersonRoles()
        {
            var initialPersonRolesFile = _options.InitialPersonRoles;
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
