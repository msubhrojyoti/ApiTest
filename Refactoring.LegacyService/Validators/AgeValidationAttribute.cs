using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Refactoring.LegacyService.Validators
{
    public class AgeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dob = value as DateTime?;
            var today = DateTime.Today;
            if (dob.HasValue)
            {
                int age = today.Year - dob.Value.Year;
                if (dob.Value.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age >= 18;
            }

            return false;
        }
    }
}
