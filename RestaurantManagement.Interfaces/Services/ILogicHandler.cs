using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Services
{
    public interface ILogicHandler
    {
        bool BooleanConverter(string value);
        List<List<string>> FileReader(string fileName);
    }
}
