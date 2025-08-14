using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;

namespace CoreLayer.CustomValidations
{
    public class EgyptianPhoneListAttribute: ValidationAttribute
    {
        private const string DefaultRegion = "EG";
        private static readonly PhoneNumberUtil PhoneUtil = PhoneNumberUtil.GetInstance();

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is not List<string> phoneNumbers)
                return ValidationResult.Success;

            var invalidNumbers = new List<string>();

            foreach (var number in phoneNumbers)
            {
                if (!IsValidEgyptianNumber(number))
                    invalidNumbers.Add(number);
            }

            return invalidNumbers.Count == 0
                ? ValidationResult.Success
                : new ValidationResult(GetErrorMessage(invalidNumbers));
        }

        private bool IsValidEgyptianNumber(string number)
        {
            try
            {
                var parsed = PhoneUtil.Parse(number, DefaultRegion);
                return PhoneUtil.IsValidNumberForRegion(parsed, DefaultRegion);
            }
            catch
            {
                return false;
            }
        }

        private string GetErrorMessage(List<string> invalidNumbers)
        {
            var invalidList = string.Join(", ", invalidNumbers.Take(3));
            var more = invalidNumbers.Count > 3 ? $" (+{invalidNumbers.Count - 3} more)" : "";
            return $"Invalid Egyptian numbers: {invalidList}{more}. " +
                   "Valid formats: 01XXXXXXXX or +201XXXXXXXXX";
        }
    }
}
