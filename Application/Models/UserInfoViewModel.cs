using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class UserInfoViewModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Create date")]
		public string? Createdate { get; set; }
        public string? Imagelink { get; set; }

        public string? Description { get; set; }
    }
}
