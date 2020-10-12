﻿using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Interfaces.Services
{
    public interface IPersonRoleService
    {
        List<PersonRole> GetInitialPersonRoles();
    }
}
