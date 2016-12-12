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
    public class IsValidZipCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isZipCode =
               Regex.IsMatch(Convert.ToString(value), @"^[1-9][0-9]{3}\s?[A-Z]{2}$");

            return isZipCode ? new ValidationResult(true, null) : new ValidationResult(false, "Geen geldige postcode!");

        }
    }
}
