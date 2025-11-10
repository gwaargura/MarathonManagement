using System;
using System.Collections.Generic;

namespace MM_api.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }
    public int UserId { get; set; }
    public int MarathonId { get; set; }
    public int? PaymentId { get; set; }
    public DateTime? RegisteredAt { get; set; }
    public string? Status { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual Marathon Marathon { get; set; } = null!;

    public virtual Payment? Payment { get; set; }

    public virtual User User { get; set; } = null!;
}
