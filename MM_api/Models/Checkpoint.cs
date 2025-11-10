using System;
using System.Collections.Generic;

namespace MM_api.Models;

public partial class Checkpoint
{
    public int CheckpointId { get; set; }
    public int MarathonId { get; set; }
    public string? Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int Sequence { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual Marathon Marathon { get; set; } = null!;
}
