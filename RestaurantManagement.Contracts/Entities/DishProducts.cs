using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class DishProducts
    {
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public decimal Portion { get; set; }

        public virtual Dishes Dish { get; set; }
        public virtual Products Product { get; set; }
    }
}
