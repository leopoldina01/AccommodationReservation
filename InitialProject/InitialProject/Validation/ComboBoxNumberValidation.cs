using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.Validation
{
    public class ComboBoxNumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                int r;

                if (int.TryParse(s, out r))
                {
                    if (r < 1)
                    {
                        return new ValidationResult(false, "Value can't be less than 1");
                    }
                    if (r > 5)
                    {
                        return new ValidationResult(false, "Value can't be greater than 5");
                    }

                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Please enter a valid int value");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }
        }
    }
}
