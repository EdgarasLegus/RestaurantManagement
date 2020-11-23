using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int OrderStatus { get; set; }
        public bool IsPreparing { get; set; }
        public bool IsReady { get; set; }

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}
