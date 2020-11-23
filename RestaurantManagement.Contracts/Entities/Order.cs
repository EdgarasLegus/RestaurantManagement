using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string OrderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int OrderStatus { get; set; }
        public bool IsPreparing { get; set; }
        public bool IsReady { get; set; }

        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
