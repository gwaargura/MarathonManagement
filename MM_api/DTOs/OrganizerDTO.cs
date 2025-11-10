namespace MM_api.DTOs
{
    using System.ComponentModel.DataAnnotations;

    namespace MM_api.DTOs
    {
        public class ReadOrganizerDTO
        {
            public string OrganizationName { get; set; } = string.Empty;
            public bool? Verified { get; set; }
            public DateTime? CreatedAt { get; set; }
        }

        public class CreateOrganizerDTO
        {
            [Required]
            public int UserId { get; set; }

            [Required]
            [StringLength(255)]
            public string OrganizationName { get; set; } = string.Empty;
        }

        public class UpdateOrganizerDTO
        {

            [Required]
            [StringLength(255)]
            public string OrganizationName { get; set; } = string.Empty;

            public bool? Verified { get; set; }
        }
    }

}
