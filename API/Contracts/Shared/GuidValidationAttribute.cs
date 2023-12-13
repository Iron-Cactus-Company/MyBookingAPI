using System;
using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Shared
{
    public class GuidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            
            if (!(value is string stringValue) || !Guid.TryParse(stringValue, out _))
            {
                return new ValidationResult(ErrorMessage ?? "Value is not a valid GUID string.");
            }

            return ValidationResult.Success;
        }
    }
}