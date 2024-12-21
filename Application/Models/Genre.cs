using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Genre
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Medium> Idmedia { get; set; } = new List<Medium>();
}
