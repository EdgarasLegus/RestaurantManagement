using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }

        public int OrderItemStatus { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Order Order { get; set; }
    }
}
