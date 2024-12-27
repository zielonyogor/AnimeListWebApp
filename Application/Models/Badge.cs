using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public partial class Badge
{
    [Required(ErrorMessage = "Badge name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "badge background color is required.")]
    public string Backgroundcolor { get; set; } = null!;

	[Required(ErrorMessage = "Badge name color name is required.")]
    public string Namecolor { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
