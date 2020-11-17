using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderUpdateModel
    {
        public string OrderName { get; set; }
        public DateTime ModifiedDate { get; } = DateTime.Now;
        public bool IsPreparing { get; set; }
        public IEnumerable<OrderItemUpdateModel> OrderItems { get; set; }
    }
}
