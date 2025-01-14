namespace Application.Models
{
    public class AuthorViewModel
    {
        public short Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Image { get; set; }

        public string? Wikipedialink { get; set; }

        public List<int> MangaIds { get; set; } = new List<int>();
    }
}
