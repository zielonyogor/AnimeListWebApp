using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Manga
{
    public int Mediumid { get; set; }

    public string? Type { get; set; }

    public short Authorid { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Medium Medium { get; set; } = null!;
}
