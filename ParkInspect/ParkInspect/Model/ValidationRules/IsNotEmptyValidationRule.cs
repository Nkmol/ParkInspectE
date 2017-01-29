using System;
using System.Globalization;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = Convert.ToString(value).Trim();

            if (!string.IsNullOrEmpty(val))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Voer een waarde in!");
        }
    }
}