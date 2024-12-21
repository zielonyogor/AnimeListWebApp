using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Listelement
{
    public int Accountid { get; set; }

    public int Mediumid { get; set; }

    public byte? Finishednumber { get; set; }

    public string Status { get; set; } = null!;

    public byte? Rating { get; set; }

    public string? Mediumcomment { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Finishdate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Medium Medium { get; set; } = null!;
}
