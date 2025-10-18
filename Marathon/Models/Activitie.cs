using System;
using System.Collections.Generic;

namespace Marathon.Models;

public partial class Activitie
{
    public int ActivityId { get; set; }

    public int Id { get; set; }

    public int? EventId { get; set; }

    public DateTime StartTime { get; set; }

    public decimal DistanceKm { get; set; }

    public int Duration { get; set; }

    public decimal? AvgPace { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Account IdNavigation { get; set; } = null!;
}
