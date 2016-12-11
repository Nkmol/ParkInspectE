using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = Convert.ToString(value).Trim();

           if (!String.IsNullOrEmpty(val))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Je mag het veld niet leeglaten!");
        }
    }
}
