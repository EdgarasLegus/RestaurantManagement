using System;

namespace RestaurantManagement.Contracts.Entities
{
    public class UserLog
    {
        public int Id { get; set; }
        public string UserAction { get; set; }
        public DateTime ActionTime { get; set; }
        public int StaffId { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
