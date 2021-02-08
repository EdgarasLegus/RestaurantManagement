using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class ProductCreateModel
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        [Required(ErrorMessage = "Stock amount is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive numbers allowed")]
        public decimal StockAmount { get; set; }
        [Required(ErrorMessage = "Unit of measurement is required")]
        public int UnitOfMeasurementId { get; set; }
    }
}
