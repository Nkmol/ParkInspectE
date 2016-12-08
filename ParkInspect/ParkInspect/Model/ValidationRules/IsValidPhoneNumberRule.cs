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
    public class IsValidPhoneNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phonenumber = Convert.ToString(value).Trim();

            bool isFixedNumber =
                Regex.IsMatch(phonenumber,
                @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)");

                return (isFixedNumber) ? new ValidationResult(true, null) : new ValidationResult(false, "Voer een geldig telnr in!");
        }
    }
}
