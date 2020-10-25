using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestaurantManagement.Contracts.Settings
{
    public class BackDateValidationChecker : ValidationAttribute
    {
        public override string FormatErrorMessage(string msg)
        {
            return "Date value should not be back date";
        }

        protected override ValidationResult IsValid(object objValue,
                                                       ValidationContext validationContext)
        {
            // Panagrineti
            var dateValue = objValue as DateTime? ?? new DateTime();
            if (dateValue.Date < DateTime.Now.Date)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
