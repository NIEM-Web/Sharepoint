using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace Niem.MyNiem.FieldValidationRules
{
  public class UserInterestFieldValidationRule
      : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double CustomName = (double)value;
            bool result = isValidCustomName(CustomName);
            if (result != true)
            {
                return new ValidationResult(false, "Enter valid Name.");
            }
            else
            {
                return new ValidationResult(true, "Name Entered is correct.");
            }
        }

        public static bool isValidCustomName(double CustomName)
        {
            //Logic to validate data
            return true;
        }
    }
}
