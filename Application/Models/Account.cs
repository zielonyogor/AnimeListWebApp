using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models;

public partial class Account : IdentityUser<int>
{
    public DateTime? Createdate { get; set; }

    public string? Imagelink { get; set; }

    public string? Description { get; set; }

    public string Accountprivilege { get; set; } = null!;

    [Required]
    public override string? PasswordHash { get; set; } = null!;

    public virtual ICollection<Listelement> Listelements { get; set; } = new List<Listelement>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Account> Accountid1s { get; set; } = new List<Account>();

    public virtual ICollection<Account> Accountid2s { get; set; } = new List<Account>();

    public virtual ICollection<Badge> Badgenames { get; set; } = new List<Badge>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();



    [NotMapped]
    public override bool TwoFactorEnabled { get; set; }

    [NotMapped]
    public override string? PhoneNumber { get; set; }

    [NotMapped]
    public override bool LockoutEnabled { get; set; }

    [NotMapped]
    public override bool PhoneNumberConfirmed { get; set; }

    [NotMapped]
    public override bool EmailConfirmed { get; set; }

    [NotMapped]
    public override DateTimeOffset? LockoutEnd { get; set; }

    [NotMapped]
    public override int AccessFailedCount { get; set; }
}
