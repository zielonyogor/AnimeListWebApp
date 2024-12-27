using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public partial class Studio
{
    [Required(ErrorMessage = "Studio name is required.")]
    [StringLength(20, ErrorMessage = "Studio name cannot exceed 20 characters.")]
    public string Name { get; set; } = null!;

    public string? Wikipedialink { get; set; }

    public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
}
