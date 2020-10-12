using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class UserLog
    {
        public int Id { get; set; }
        public string UserAction { get; set; }
        public DateTime ActionTime { get; set; }
        public int StaffId { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
