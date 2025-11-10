using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc; // <-- Required for ValidateComplexType

namespace MM_client.Models
{
    public class RegisterViewModel
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public bool IsOrganizer { get; set; }

        [Display(Name = "Tên tổ chức")]
        public string? OrganizationName { get; set; }
    }

    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
