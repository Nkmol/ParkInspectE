using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsValidPhoneNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var phonenumber = Convert.ToString(value).Trim();

            var isFixedNumber =
                Regex.IsMatch(phonenumber,
                    @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)");

            return isFixedNumber
                ? new ValidationResult(true, null)
                : new ValidationResult(false, "Voer een geldig telefoonnummer in!");
        }
    }
}