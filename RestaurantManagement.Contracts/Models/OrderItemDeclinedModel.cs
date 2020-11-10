using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderItemDeclinedModel
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }
        public int OrderItemStatus { get; } = (int)OrderStates.Declined;
    }
}
