namespace MM_client.Models
{
    public partial class Organizer
    {
        public int OrganizerId { get; set; }

        public int UserId { get; set; }

        public string OrganizationName { get; set; } = null!;

        public bool? Verified { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
