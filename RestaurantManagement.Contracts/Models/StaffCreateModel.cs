using RestaurantManagement.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantManagement.Contracts.Models
{
    public class StaffCreateModel
    {
        [StringLength(50, ErrorMessage = "Must be between 1 and 50 characters", MinimumLength = 1)]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Must be between 5 and 50 characters", MinimumLength = 5)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int PersonRoleId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDayOfEmployment { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [BackDateValidationChecker]
        public DateTime EndDayOfEmployment { get; set; }
    }
}
