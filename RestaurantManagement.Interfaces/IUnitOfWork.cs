using RestaurantManagement.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDishProductRepo DishProductRepo { get; }
        IDishRepo DishRepo { get; }
        IOrderItemRepo OrderItemRepo { get; }
        IOrderRepo OrderRepo { get; }
        IPersonRoleRepo PersonRoleRepo { get; }
        IProductRepo ProductRepo { get; }
        IRestaurantTableRepo RestaurantTableRepo { get; }
        IStaffRepo StaffRepo { get; }
        IUserLogRepo UserLogRepo { get; }
        Task<int> Commit();
    }
}
