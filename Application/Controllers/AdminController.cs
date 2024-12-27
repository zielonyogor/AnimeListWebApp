using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var accounts = await _userManager.Users
                .Select(u => new AccountViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Createdate = u.Createdate,
                    Imagelink = u.Imagelink,
                    Description = u.Description
                })
                .ToListAsync();

            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(AccountViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("", "Username cannot be empty");
                return RedirectToAction("Accounts");
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Description = model.Description;
            user.Imagelink = model.Imagelink;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update account details");
            }

            return RedirectToAction("Accounts");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to delete user");
            }

            return RedirectToAction("Accounts");
        }
    }
}
