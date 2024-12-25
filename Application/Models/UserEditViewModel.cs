using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class UserEditViewModel
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
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [StringLength(20)]
        [Display(Name = "New Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? Imagelink { get; set; }

        public string? Description { get; set; }
    }

}
