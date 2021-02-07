using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public class UnitOfMeasurement
    {
        public UnitOfMeasurement()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string UnitName { get; set; }
        public string UnitDescription { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
