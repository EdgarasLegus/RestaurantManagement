using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IProductService
    {
        List<Product> GetInitialProducts();
    }
}
