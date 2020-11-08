using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class DishViewModel
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public bool IsOnMenu { get; set; }
        public string DishType { get; set; }
        public int QuantityInStock { get; set; }
        public IEnumerable<DishProductViewModel> DishProduct { get; set; }
    }
}
