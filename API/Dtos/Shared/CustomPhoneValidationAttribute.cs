using System;
using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace API.Dtos.Shared
{
    [Flags]
    public enum SupportedRegion
    {
        Finland = 1,
        UnitedStates = 2,
    }
    
    public class CustomPhoneValidationAttribute : ValidationAttribute
    {
        private SupportedRegion Regions { get; }

        public CustomPhoneValidationAttribute(SupportedRegion regions)
        {
            Regions = regions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; 
            }

            if (Regions == 0) 
            {
                return ValidationResult.Success;
            }

            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            foreach (SupportedRegion region in Enum.GetValues(typeof(SupportedRegion)))
            {
                if (Regions.HasFlag(region))
                {
                    var regionCode = GetRegionCode(region);
                    var phoneNumber = phoneNumberUtil.Parse((string)value, regionCode);

                    if (phoneNumberUtil.IsValidNumber(phoneNumber))
                    {
                        return ValidationResult.Success;
                    }
                }
            }

            return new ValidationResult(ErrorMessage);
        }

        private string GetRegionCode(SupportedRegion region)
        {
            switch (region)
            {
                case SupportedRegion.Finland:
                    return "FI";
                case SupportedRegion.UnitedStates:
                    return "US";
                default:
                    throw new ArgumentOutOfRangeException(nameof(region), region, null);
            }
        }
    }
}