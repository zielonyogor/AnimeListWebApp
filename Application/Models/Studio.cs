using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Studio
{
    public string Name { get; set; } = null!;

    public string? Wikipedialink { get; set; }

    public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
}
