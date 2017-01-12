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
            int i;
            if(int.TryParse(value.ToString(), out i))
             if (i > 0)
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Het moet een getal hoger dan 0 zijn!");
        }
    }
}
