using System;
using System.Collections.Generic;

namespace Marathon.Models;

public partial class Organizer
{
    public int OrganizerId { get; set; }

    public string OrgName { get; set; } = null!;

    public string? ContactEmail { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
