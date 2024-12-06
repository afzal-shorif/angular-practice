using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagement.Application.Configurations.ValidationAttributes
{
    public class Username : ValidationAttribute
    {
        public Username()
        {
            const string defaultErrorMessage = "Error with username";
            ErrorMessage ??= defaultErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Regex regex = new Regex("^[A-Z]+$", RegexOptions.IgnoreCase);

            if (!regex.IsMatch(value.ToString()))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
