using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Enums
{
    public enum OrderItemStates
    {
        Created = 10,
        Declined = 30,
        Preparing = 50,
        Completed = 80,
        Cancelled = 90
    }
}
