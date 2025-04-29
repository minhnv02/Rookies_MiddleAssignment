using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MidAssignment.Domain.Constants;

namespace MidAssignment.Application.Models.Requests
{
    public class UserRegisterRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression(RegexConstants.PASSWORD, ErrorMessage = ErrorMessage.ERROR_PASSWORD)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = ErrorMessage.ERROR_CONFIRM_PASSWORD)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Range(1, 2)]
        public int RoleId { get; set; }
    }
}
