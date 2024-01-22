using System.ComponentModel.DataAnnotations;

namespace GuestAPI.Attribute
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Date >= DateTime.Today)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class DepartureDateAttribute(string arrivalDatePropertyName) : ValidationAttribute
    {
        private readonly string _arrivalDatePropertyName = arrivalDatePropertyName;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var arrivalDateProperty = validationContext.ObjectType.GetProperty(_arrivalDatePropertyName);
            if (arrivalDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {_arrivalDatePropertyName}");
            }

            var arrivalDateValue = arrivalDateProperty.GetValue(validationContext.ObjectInstance);
            if (arrivalDateValue == null || arrivalDateValue is not DateTime arrivalDate)
            {
                return new ValidationResult("Arrival date is not set or not a valid date");
            }

            if (value is not DateTime departureDate)
            {
                return new ValidationResult("Departure date is not set or not a valid date");
            }

            if (departureDate.Date <= arrivalDate.Date)
            {
                return new ValidationResult("Departure date must be greater than arrival date");
            }

            return ValidationResult.Success!;
        }
    }
}

