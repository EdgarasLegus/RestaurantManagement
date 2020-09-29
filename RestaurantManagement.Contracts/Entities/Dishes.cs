using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public class Dishes
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string DishType { get; set; }
        public bool IsOnMenu { get; set; }
    }
}
