using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email or Username")]
        public string EmailOrUsername { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
    }
}
