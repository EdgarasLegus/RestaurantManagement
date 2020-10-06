using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Contracts.Interfaces.Repositories
{
    public interface IProductRepo
    {
        Task InsertInitialProducts(List<Product> productColumns);
    }
}
