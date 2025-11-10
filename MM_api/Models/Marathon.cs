using System;
using System.Collections.Generic;

namespace MM_api.Models;

public partial class Marathon
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
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();

    public virtual Organizer Organizer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
