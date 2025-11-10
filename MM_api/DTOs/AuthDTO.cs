using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MM_api.DTOs.MM_api.DTOs;

namespace MM_api.DTOs
{
    public class LoginRequestDTO
    {
        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
    }
    public class RegisterRequest
    {
        public CreateUserDTO User { get; set; } = null!;
        public bool IsOrganizer { get; set; }
        public CreateOrganizerDTO? Organizer { get; set; }
    }


    public class TokenRequestDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
