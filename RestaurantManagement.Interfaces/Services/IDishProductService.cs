using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace RestaurantManagement.Interfaces.Services
{
    public interface IDishProductService
    {
        List<DishProduct> GetInitialDishProducts();
    }
}
