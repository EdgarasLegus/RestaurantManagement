using System;
using System.Threading.Tasks;

namespace RestaurantManagement.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
        IRepository<T> GetRepository<T>() where T : class;
    }
}
