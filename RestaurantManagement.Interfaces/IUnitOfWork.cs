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
        Task<int> Commit();
        IRepository<T> GetRepository<T>() where T : class;
    }
}
