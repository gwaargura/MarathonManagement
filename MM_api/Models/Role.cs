using System;
using System.Collections.Generic;

namespace MM_api.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
