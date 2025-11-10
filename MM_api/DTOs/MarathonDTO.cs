namespace MM_api.DTOs
{
    using System;

    namespace MM_api.DTOs
    {
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

        public class CreateMarathonDTO
        {
            public int OrganizerId { get; set; }
            public string MarathonName { get; set; } = null!;
            public string? Description { get; set; }
            public string? Location { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal RegistrationFee { get; set; }
            public int? MaxParticipants { get; set; }
            public string? Status { get; set; }
        }

        public class UpdateMarathonDTO
        {
            public string? MarathonName { get; set; }
            public string? Description { get; set; }
            public string? Location { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public decimal? RegistrationFee { get; set; }
            public int? MaxParticipants { get; set; }
            public string? Status { get; set; }
        }
    }

}
