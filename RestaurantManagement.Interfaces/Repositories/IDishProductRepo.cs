using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces.Repositories
{
    public interface IDishProductRepo : IRepository<DishProduct>
    {
        Task InsertInitialDishProducts(List<DishProduct> dishProductList);
    }
}
