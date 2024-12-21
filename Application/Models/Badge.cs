using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Badge
{
    public string Name { get; set; } = null!;

    public string Backgroundcolor { get; set; } = null!;

    public string Namecolor { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
