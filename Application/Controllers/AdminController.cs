using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[Authorize(Roles = "Admin,Moderator")]
	public class AdminController : Controller
	{
		private readonly UserManager<Account> _userManager;
		private readonly RoleManager<IdentityRole<int>> _roleManager;

		public AdminController(UserManager<Account> userManager, RoleManager<IdentityRole<int>> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> AdminPanel()
		{
			var mods = (await _userManager.GetUsersInRoleAsync("Moderator"))
						.Select(u => new
						{
							u.Id,
							u.UserName,
							u.Email
						})
						.ToList();
			return View(mods);
		}

		public IActionResult ModPanel()
		{
			return View();
		}

        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> RemoveModerator(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null && await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Moderator");
            }
            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddModerator(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                await _userManager.AddToRoleAsync(user, "Moderator");
            }
            return RedirectToAction("AdminPanel");
        }

        [Authorize(Roles = "Admin")]
		public IActionResult Users()
		{
			return View();
		}

        [Authorize(Roles = "Admin")]
        public IActionResult Badges()
        {
            return View();
        }
        public IActionResult Genres()
        {
            return View();
        }
        public IActionResult Characters()
        {
            return View();
        }
        public IActionResult Studios()
        {
            return View();
        }
        public IActionResult Authors()
        {
            return View();
        }
        public IActionResult Anime()
        {
            return View();
        }
        public IActionResult Mangas()
        {
            return View();
        }
    }
}
