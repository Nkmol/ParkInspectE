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
            string format = "d-M-yyyy";
            if (DateTime.TryParseExact(Convert.ToString(value), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Voer een geldige datum in!");
        }
    }
}
