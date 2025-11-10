using System;
using System.Collections.Generic;

namespace MM_api.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int ActorId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public string? TargetEntity { get; set; }

    public int? TargetId { get; set; }

    public virtual User Actor { get; set; } = null!;
}
