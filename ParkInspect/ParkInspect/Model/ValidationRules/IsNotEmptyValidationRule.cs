using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsNotEmptyValidationRule : ValidationRule
    {
        public UIElement Element { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(Element != null && !Element.IsEnabled)
                return new ValidationResult(true, null);

            var val = Convert.ToString(value).Trim();

            if (!string.IsNullOrEmpty(val))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Voer een waarde in!");
        }
    }
}