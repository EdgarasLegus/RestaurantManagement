using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class Reservations
    {
        public int Id { get; set; }
        public string ReservationPersonName { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationFrom { get; set; }
        public DateTime ReservationUntil { get; set; }
        public int ReservationStatus { get; set; }

        public virtual RestaurantTables Table { get; set; }
    }
}
