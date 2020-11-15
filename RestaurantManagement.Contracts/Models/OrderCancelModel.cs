using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderCancelModel
    {
        public DateTime ModifiedDate { get; } = DateTime.Now;
        public int OrderStatus { get; } = (int)OrderStates.Cancelled;

        public IEnumerable<OrderItemCreateModel> OrderItems { get; set; }
    }
}
