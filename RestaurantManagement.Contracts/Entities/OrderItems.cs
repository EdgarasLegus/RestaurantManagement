using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class OrderItems
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }

        public virtual Dishes Dish { get; set; }
        public virtual Orders Order { get; set; }
    }
}
