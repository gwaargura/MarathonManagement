using System;
using System.Collections.Generic;

namespace Marathon.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int OrganizerId { get; set; }

    public string EventName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal? RegistrationFee { get; set; }

    public virtual ICollection<Activitie> Activities { get; set; } = new List<Activitie>();

    public virtual Organizer Organizer { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<RouteCheckpoint> RouteCheckpoints { get; set; } = new List<RouteCheckpoint>();
}
