namespace MM_client.Models
{
    public class MarathonIndexViewModel
    {
        public List<ReadMarathonDTO> AllMarathons { get; set; } = new();
        public List<ReadMarathonDTO> MyMarathons { get; set; } = new();
    }
    public class ReadMarathonDetailDTO
    {
        public int MarathonId { get; set; }
        public string OrganizerName { get; set; }
        public string MarathonName { get; set; } = null!;
        public string ThumbnailLink { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RegistrationFee { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Status { get; set; }

        public List<ReadCheckpointDTO> Checkpoints { get; set; } = new List<ReadCheckpointDTO>();
    }

    public class ReadCheckpointDTO
    {
        public string? Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Sequence { get; set; }
    }
    public class ReadMarathonDTO
    {
        public int MarathonId { get; set; }
        public int OrganizerId { get; set; }
        public string MarathonName { get; set; } = null!;
        public string ThumbnailLink { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RegistrationFee { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class CreateMarathonDTO { public int OrganizerId { get; set; } public string MarathonName { get; set; } = null!; public string? Description { get; set; } public string? Location { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public decimal RegistrationFee { get; set; } public int? MaxParticipants { get; set; } public string? Status { get; set; } }
    public class UpdateMarathonDTO { public string? MarathonName { get; set; } public string? Description { get; set; } public string? Location { get; set; } public DateTime? StartDate { get; set; } public DateTime? EndDate { get; set; } public decimal? RegistrationFee { get; set; } public int? MaxParticipants { get; set; } public string? Status { get; set; } }
    public class CreateCheckpointDTO { public int MarathonId { get; set; } public string? Name { get; set; } public decimal Latitude { get; set; } public decimal Longitude { get; set; } public int Sequence { get; set; } } // Update DTO — for updating existing checkpoints public class UpdateCheckpointDTO { public string? Name { get; set; } public decimal? Latitude { get; set; } public decimal? Longitude { get; set; } public int? Sequence { get; set; } }
}
