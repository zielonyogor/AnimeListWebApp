namespace Application.Models
{
	public class StudioViewModel
	{
		public string Name { get; set; } = null!;

		public string? Wikipedialink { get; set; }

		public List<int> AnimeIds { get; set; } = new List<int>();
	}
}
