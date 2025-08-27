using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;

namespace CoreLayer.CustomValidations
{

    public class EgyptianPhoneAttribute : ValidationAttribute
    {
        private const string DefaultRegion = "EG";
        private static readonly PhoneNumberUtil PhoneUtil = PhoneNumberUtil.GetInstance();

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is null or string { Length: 0 })
                return ValidationResult.Success; // optional: allow empty if nullable

            var number = value as string;

            if (!IsValidEgyptianNumber(number))
                return new ValidationResult($"Invalid Egyptian phone number: {number}. " +
                                            "Valid formats: 01XXXXXXXX or +201XXXXXXXXX");

            return ValidationResult.Success;
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
    }

}
