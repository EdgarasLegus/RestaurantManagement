using NLog;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    // your own implementation is not needed. You can use build in logging
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        public void LogError(string message)
        {
            logger.Error(message);
        }
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
