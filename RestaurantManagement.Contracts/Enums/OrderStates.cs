using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Enums
{
    public enum OrderStates
    {
        Created = 10,
        PartiallyDeclined = 20,
        Declined = 30,
        Edited = 40,
        Preparing = 50,
        ReadyToServe = 60,
        Updated = 70,
        Completed = 80,
        Cancelled = 90
    }
}
