using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsNotIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "Numerieke karakters zijn niet toegestaan!");
            var chars = value.ToString().ToCharArray();
            var s = "1234567890";

            return chars.Any(c => s.Contains(c))
                ? new ValidationResult(false, "Numerieke karakters zijn niet toegestaan!")
                : new ValidationResult(true, null);
        }
    }
}