using System;
using System.Collections.Generic;

namespace MM_api.Models;

public partial class Payment
{
    public int PaymentId { get; set; }
    public int UserId { get; set; }
    public int MarathonId { get; set; }
    public string? VnpayTransactionId { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ResponseCode { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual Marathon Marathon { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual User User { get; set; } = null!;
}
