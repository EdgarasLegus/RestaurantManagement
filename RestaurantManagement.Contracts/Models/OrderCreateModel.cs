using RestaurantManagement.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderCreateModel
    {
        [StringLength(50, ErrorMessage = "Must be between 1 and 50 characters", MinimumLength = 1)]
        [Required(ErrorMessage = "OrderName is required")]
        public string OrderName { get; set; }
        public DateTime CreatedDate { get; } = DateTime.Now;
        public DateTime ModifiedDate { get;} = DateTime.Now;
        public int OrderStatus { get; } = (int)OrderStates.Created;

        public IEnumerable<OrderItemCreateModel> OrderItems { get; set; }
    }
}
