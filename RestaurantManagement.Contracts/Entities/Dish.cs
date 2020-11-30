using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public class Dish
    {
        public Dish()
        {
            DishProduct = new HashSet<DishProduct>();
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string DishName { get; set; }
        public bool IsOnMenu { get; set; }
        public string DishType { get; set; }
        public int QuantityInStock { get; set; }

        public virtual ICollection<DishProduct> DishProduct { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
