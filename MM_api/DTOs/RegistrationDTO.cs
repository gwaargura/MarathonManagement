namespace MM_api.DTOs
{
    public class CreateRegistrationDTO
    {
        public int UserId { get; set; }
        public int MarathonId { get; set; }
        public int? PaymentId { get; set; }
        public string? Status { get; set; }
    }

    public class ReadRegistrationDTO
    {
        public int RegistrationId { get; set; }
        public int UserId { get; set; }
        public int MarathonId { get; set; }
        public int? PaymentId { get; set; }
        public DateTime? RegisteredAt { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateRegistrationDTO
    {
        public string? Status { get; set; }
        public int? PaymentId { get; set; }
    }
}
