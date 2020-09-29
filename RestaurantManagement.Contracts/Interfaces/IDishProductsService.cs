using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace RestaurantManagement.Contracts
{
    public interface IDishProductsService
    {
        List<DishProducts> GetInitialDishProducts();
    }
}
