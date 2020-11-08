using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderPartiallyDeclinedModel
    {
        public string OrderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; } = DateTime.Now;
        public int OrderStatus { get; } = (int)OrderStates.PartiallyDeclined;

        public IEnumerable<OrderItemPartiallyDeclinedModel> OrderItems { get; set; }
    }
}
