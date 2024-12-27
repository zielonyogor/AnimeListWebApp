using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;
        public DateTime? Createdate { get; set; }

        public string? Imagelink { get; set; }

        public string? Description { get; set; }

    }
}
