using RestaurantManagement.Contracts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class LogicHandler : ILogicHandler
    {
        public Boolean BooleanConverter(string value)
        {
            bool convertedValue;
            if(value == "1")
            {
                convertedValue = true;
            }
            else
            {
                convertedValue = false;
            }
            return convertedValue;
        }
    }
}
