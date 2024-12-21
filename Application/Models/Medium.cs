using System;
using System.Collections.Generic;

namespace Application.Models;

public partial class Medium
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Status { get; set; }

    public byte Count { get; set; }

    public string? Poster { get; set; }

    public DateTime? Publishdate { get; set; }

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public virtual Anime? Anime { get; set; }

    public virtual ICollection<Listelement> Listelements { get; set; } = new List<Listelement>();

    public virtual Manga? Manga { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<Genre> Genrenames { get; set; } = new List<Genre>();

    public virtual ICollection<Medium> Idmedium1s { get; set; } = new List<Medium>();

    public virtual ICollection<Medium> Idmedium2s { get; set; } = new List<Medium>();
}
