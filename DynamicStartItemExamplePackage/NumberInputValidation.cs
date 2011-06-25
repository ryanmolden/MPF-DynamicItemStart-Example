using System;
using System.Windows.Controls;

namespace ExampleInc.DynamicStartItemExamplePackage
{
    class NumberInputValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string typedValue = (string)value;
            int convertedValue;
            if (Int32.TryParse(typedValue, out convertedValue)) 
            {
                if (convertedValue > 0 && convertedValue <= 100)
                {
                    return new ValidationResult(isValid: true, errorContent: null);
                }
                else
                {
                    return new ValidationResult(isValid: false, errorContent: "The value entered must be > 0 and <= 100.");
                }
            }

            return new ValidationResult(isValid: false, errorContent: "You  must enter a valid numerical value.");
        }
    }
}