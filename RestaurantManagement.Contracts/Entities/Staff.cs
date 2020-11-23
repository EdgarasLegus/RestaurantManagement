using RestaurantManagement.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RestaurantManagement.Contracts.Entities
{
    // Partial nereikia naudoti
    public class Staff
    {
        public Staff()
        {
            UserLog = new HashSet<UserLog>();
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Must be between 1 and 50 characters", MinimumLength = 1)]
        // Resurso failas vietoj hardcoding
        [Required(ErrorMessage = "Username is required")]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int PersonRoleId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        //[RegularExpression(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date is invalid")]
        [BackDateValidationChecker]
        public DateTime StartDayOfEmployment { get; set; }

        [Required(ErrorMessage = "End date is required")]
        //[RegularExpression(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date is invalid")]
        [BackDateValidationChecker]
        public DateTime EndDayOfEmployment { get; set; }

        // Nenaudojamas, galima be sito
        public virtual PersonRole PersonRole { get; set; }

        public virtual ICollection<UserLog> UserLog { get; set; }
    }
}
