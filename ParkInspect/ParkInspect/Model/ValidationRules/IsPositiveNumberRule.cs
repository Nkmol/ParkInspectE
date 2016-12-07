using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsPositiveNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && value is int)
            {
                if((int) value > 0)
                    return new ValidationResult(true, null);
            }

            return new ValidationResult(false, "Het moet een getal hoger dan 0 zijn!");
        }
    }
}
