using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class ListElementViewModel
    {
        [Required]
        public int Mediumid { get; set; }

        public byte? Finishednumber { get; set; }

        public string Status { get; set; } = null!;

        public byte? Rating { get; set; }

		[StringLength(200, ErrorMessage = "Comment cannot exceed 200 characters.")]
		public string? Mediumcomment { get; set; }

        public DateTime? Startdate { get; set; }

        public DateTime? Finishdate { get; set; }
    }
}
