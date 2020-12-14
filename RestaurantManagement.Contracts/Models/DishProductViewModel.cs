using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    // move lla view models to Web app
    public class DishProductViewModel
    {
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public decimal Portion { get; set; }
    }
}
