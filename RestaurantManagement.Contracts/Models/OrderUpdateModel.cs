using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderUpdateModel
    {
        public DateTime ModifiedDate { get; } = DateTime.Now;
        public int OrderStatus { get; } = (int)OrderStates.Updated;

        public IEnumerable<OrderItemUpdateModel> OrderItems { get; set; }
    }
}
