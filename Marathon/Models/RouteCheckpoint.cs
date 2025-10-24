using System;
using System.Collections.Generic;

namespace Marathon.Models;

public partial class RouteCheckpoint
{
    public int CheckpointId { get; set; }

    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public decimal DistanceFromStart { get; set; }

    public virtual Event Event { get; set; } = null!;
}
