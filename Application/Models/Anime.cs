using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Anime
{
    public int Mediumid { get; set; }

    public string? Type { get; set; }

    public string Studioname { get; set; } = null!;

    public virtual Medium Medium { get; set; } = null!;

    public virtual Studio StudionameNavigation { get; set; } = null!;
}
