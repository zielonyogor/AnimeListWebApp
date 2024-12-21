using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Author
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? Wikipedialink { get; set; }

    public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}
