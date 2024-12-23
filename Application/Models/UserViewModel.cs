using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
	public class UserViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;

		[Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
        [Display(Name = "Username")]
		public string UserName { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		[StringLength(20)]
		[Display(Name = "Password")]
		public string Password { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; } = null!;

		public string? Imagelink { get; set; }

		public string? Description { get; set; }

	}
}
