using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class DishProduct
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public decimal Portion { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Product Product { get; set; }
    }
}
