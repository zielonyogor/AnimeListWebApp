namespace Application.Models
{
    public class AnimeViewModel
    {
        public string Name { get; set; } = null!;

        public string? Status { get; set; }

        public byte Count { get; set; }

        public string? Poster { get; set; }

        public DateTime? Publishdate { get; set; }

        public string? Description { get; set; }

        public string? Type { get; set; }

        public string Studioname { get; set; } = null!;

        public virtual ICollection<Genre> Genrenames { get; set; } = new List<Genre>();

        public virtual ICollection<Medium> Connections { get; set; } = new List<Medium>();
    }
}
