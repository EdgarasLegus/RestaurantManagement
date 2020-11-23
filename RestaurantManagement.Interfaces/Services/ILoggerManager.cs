using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Services
{

    // this interface is not needed. use built in ILogger<T>
    public interface ILoggerManager
    {
        void LogDebug(string message);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarn(string message);
    }
}
