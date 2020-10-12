using RestaurantManagement.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace RestaurantManagement.Contracts.Entities
{
    public partial class PersonRole
    {
        public PersonRole()
        {
            Staff = new HashSet<Staff>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Staff> Staff { get; set; }
    }
}
