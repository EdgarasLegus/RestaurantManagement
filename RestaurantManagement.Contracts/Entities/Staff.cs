using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class Staff
    {
        public Staff()
        {
            UserLog = new HashSet<UserLog>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
        public DateTime StartDayOfEmployment { get; set; }
        public DateTime EndDayOfEmployment { get; set; }

        public virtual ICollection<UserLog> UserLog { get; set; }
    }
}
