using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal StockAmount { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
