using Microsoft.AspNetCore.Mvc;

namespace MM_client.Models
{
        public class ReadUserDTO
        {
            public int UserId { get; set; }
            public string Username { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string? FullName { get; set; }
            public int RoleId { get; set; }
            public bool? IsActive { get; set; }
            public DateTime? CreatedAt { get; set; }
        }
        public class UpdateUserDTO
        {
            public string? PasswordHash { get; set; }
            public string? Email { get; set; }
            public string? FullName { get; set; }
            public bool IsActive { get; set; }
        }
}
