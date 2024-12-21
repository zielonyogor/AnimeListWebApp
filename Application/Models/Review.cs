using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Review
{
    public int Accountid { get; set; }

    public int Mediumid { get; set; }

    public string Description { get; set; } = null!;

    public string Feeling { get; set; } = null!;

    public DateTime Postdate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Medium Medium { get; set; } = null!;
}
