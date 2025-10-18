using System;
using System.Collections.Generic;

namespace Marathon.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int Id { get; set; }

    public int EventId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string? Status { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Account IdNavigation { get; set; } = null!;
}
