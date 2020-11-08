using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class StaffViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int PersonRoleId { get; set; }
        public DateTime StartDayOfEmployment { get; set; }
        public DateTime EndDayOfEmployment { get; set; }
    }
}
