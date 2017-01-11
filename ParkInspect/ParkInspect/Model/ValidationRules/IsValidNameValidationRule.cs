using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    class IsValidNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = Convert.ToString(value).Trim();
            var isValid = value != null && !string.IsNullOrEmpty(val) && value.ToString().All(char.IsLetter); ;

            return isValid ? new ValidationResult(true, null) : new ValidationResult(false, "Voer een geldige naam in!");
        }
    }
}
