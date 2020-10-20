using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IRestaurantTableRepo
    {
        Task InsertInitialRestaurantTables(List<RestaurantTable> tablesList);
    }
}
