using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Character
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();
}
