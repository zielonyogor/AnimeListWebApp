namespace Application.Models;

public class ReviewViewModel
{
	public string? UserName { get; set; }
	public int MediumId { get; set; }
	public string Name { get; set; } = null!;
	public string Type { get; set; } = null!;
	public DateTime? PublishDate { get; set; }

	public string? Description { get; set; }

	public string? Feeling { get; set; }

	public DateTime Postdate { get; set; }
	public string? ReturnUrl { get; set; }
}
