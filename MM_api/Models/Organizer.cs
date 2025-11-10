using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MM_api.Models;

public partial class Organizer
{
    public int OrganizerId { get; set; }
    public int UserId { get; set; }
    public string OrganizationName { get; set; } = null!;
    public bool? Verified { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Marathon> Marathons { get; set; } = new List<Marathon>();
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
