using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public class Product
    {
        public Product()
        {
            DishProduct = new HashSet<DishProduct>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal StockAmount { get; set; }
        public string UnitOfMeasure { get; set; }

        public virtual ICollection<DishProduct> DishProduct { get; set; }
    }
}
