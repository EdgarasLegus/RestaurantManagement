using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal StockAmount { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
