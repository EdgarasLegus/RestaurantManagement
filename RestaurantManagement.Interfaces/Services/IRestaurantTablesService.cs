using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IRestaurantTablesService
    {
        List<RestaurantTable> GetInitialRestaurantTables();
    }
}
