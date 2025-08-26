using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.CustomValidations
{
    public class EitherOrAttribute : ValidationAttribute
    {
        private readonly string _firstProperty;
        private readonly string _secondProperty;

        public EitherOrAttribute(string firstProperty, string secondProperty)
        {
            _firstProperty = firstProperty;
            _secondProperty = secondProperty;
            ErrorMessage = $"{firstProperty} and {secondProperty} cannot both have values at the same time.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var firstProp = validationContext.ObjectType.GetProperty(_firstProperty);
            var secondProp = validationContext.ObjectType.GetProperty(_secondProperty);

            if (firstProp == null || secondProp == null)
                return new ValidationResult($"Properties '{_firstProperty}' or '{_secondProperty}' not found.");

            var firstVal = firstProp.GetValue(validationContext.ObjectInstance);
            var secondVal = secondProp.GetValue(validationContext.ObjectInstance);

            bool firstHasValue = HasValue(firstVal);
            bool secondHasValue = HasValue(secondVal);

            if (firstHasValue && secondHasValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private bool HasValue(object? val)
        {
            if (val == null) return false;

            if (val is int intVal)
                return intVal != 0;
            if (val is double dblVal)
                return dblVal != 0.0;

            return true; // fallback
        }
    }
}
