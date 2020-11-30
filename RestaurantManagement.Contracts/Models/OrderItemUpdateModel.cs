using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderItemUpdateModel
    {
        public int DishId { get; set; }
        [Range(1, 10, ErrorMessage = "Ordered quantity must be from {1} to {2}!")]
        public int Quantity { get; set; }
        public bool IsServed { get; set; }
    }
}
