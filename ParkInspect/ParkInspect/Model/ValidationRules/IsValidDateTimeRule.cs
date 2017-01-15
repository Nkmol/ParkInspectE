using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsValidDateTimeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime temp;

            // WPF transforms value to general format, checking on format should not be necessary
            if (DateTime.TryParse(Convert.ToString(value), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out temp))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Voer een geldige datum in!");
        }
    }
}
