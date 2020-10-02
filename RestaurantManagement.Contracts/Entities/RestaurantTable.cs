using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class RestaurantTable
    {
        public RestaurantTable()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string TableName { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
