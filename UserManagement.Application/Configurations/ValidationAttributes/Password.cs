using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagement.Application.Configurations.ValidationAttributes
{
    public class Password : ValidationAttribute
    {
        public Password()
        {
            const string defaultErrorMessage = "Error with password";
            ErrorMessage ??= defaultErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Regex regex = new Regex("^.*(?=.{8,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$", RegexOptions.IgnoreCase);

            if (!regex.IsMatch(value.ToString()))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
