using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class RestaurantTables
    {
        public RestaurantTables()
        {
            Reservations = new HashSet<Reservations>();
        }

        public int Id { get; set; }
        public string TableName { get; set; }

        public virtual ICollection<Reservations> Reservations { get; set; }
    }
}
