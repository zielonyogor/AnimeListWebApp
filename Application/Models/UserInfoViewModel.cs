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

		public List<CharacterViewModel> Characters { get; set; } = new List<CharacterViewModel>();
        public List<Account> Followers { get; set; } = new List<Account>();
        public List<Account> Following { get; set; } = new List<Account>();
	}
}
