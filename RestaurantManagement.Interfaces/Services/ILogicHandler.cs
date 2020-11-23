using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Interfaces.Services
{
    // name does not make sense. not much logic here. these can be extension methods or helper classes
    public interface ILogicHandler
    {
        Boolean BooleanConverter(string value);
        List<List<string>> FileReader(string fileName);
    }
}
