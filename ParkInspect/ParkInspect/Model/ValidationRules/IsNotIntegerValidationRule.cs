using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkInspect.Model.ValidationRules
{
    public class IsNotIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int i;
                if(value != null && !int.TryParse((string)value, out i))
                    return new ValidationResult(true, null);

            return new ValidationResult(false, "Numerieke karakters zijn niet toegestaan!");
        }
    }
}
