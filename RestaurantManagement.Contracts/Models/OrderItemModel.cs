﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class OrderItemModel
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }

    }
}