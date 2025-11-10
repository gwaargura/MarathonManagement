using System.ComponentModel.DataAnnotations;

namespace MM_api.DTOs
{
    public class ReadRoleDTO
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class CreateRoleDTO
    {
        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;
    }

    public class UpdateRoleDTO
    {
        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;
    }
}
