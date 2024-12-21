using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Account
{
    public int Accountid { get; set; }

    public string Emailaddress { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? Createdate { get; set; }

    public string? Imagelink { get; set; }

    public string? Description { get; set; }

    public string Accountprivilege { get; set; } = null!;

    public virtual ICollection<Listelement> Listelements { get; set; } = new List<Listelement>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Account> Accountid1s { get; set; } = new List<Account>();

    public virtual ICollection<Account> Accountid2s { get; set; } = new List<Account>();

    public virtual ICollection<Badge> Badgenames { get; set; } = new List<Badge>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}
