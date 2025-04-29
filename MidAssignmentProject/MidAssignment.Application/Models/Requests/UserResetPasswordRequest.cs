using MidAssignment.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace MidAssignment.Application.Models.Requests
{
    public class UserResetPasswordRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(RegexConstants.PASSWORD, ErrorMessage = ErrorMessage.ERROR_PASSWORD)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = ErrorMessage.ERROR_CONFIRM_PASSWORD)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
