using Application.Data;
using Application.Misc;
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
        private readonly ModelContext _context;

		public AdminController(UserManager<Account> userManager, RoleManager<IdentityRole<int>> roleManager, ModelContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
            _context = context;
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
		public async Task<IActionResult> Reviews()
		{
            var reviews = await _context.Reviews
                .Include(r => r.Account)
                .Include(r => r.Medium)
                .ToListAsync();
			return View(reviews);
		}

        [HttpPost]
        public async Task<IActionResult> DeleteReview(int accountId, int mediumId)
        {
            var review = await _context.Reviews
                .Include(r => r.Account)
                .Include(r => r.Medium)
                .Where(r => r.Accountid == accountId && r.Mediumid == mediumId)
                .FirstOrDefaultAsync();

            if(review == null)
                return RedirectToAction("Reviews");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Reviews");
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
                return RedirectToAction("Users");
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

            return RedirectToAction("Users");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount([FromQuery] int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { Message = "User not found" });

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(new { Message = "Failed to delete user from identity", Errors = identityResult.Errors });
                }

                Helper.DeleteImage(user.Imagelink);
                
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An error occurred while deleting the user", Details = ex.Message });
            }

            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<IActionResult> UsersBadges()
        {
			var accounts = await _userManager.Users
                .Include(u => u.Badgenames)
				.Select(u => new AccountViewModel
				{
					Id = u.Id,
					UserName = u.UserName,
					Email = u.Email,
					Createdate = u.Createdate,
					Imagelink = u.Imagelink,
					Description = u.Description,
                    Badgenames = u.Badgenames.ToList(),
				})
				.ToListAsync();

			return View(accounts);
		}

        [HttpPost]
        public async Task<IActionResult> AddBadge(int accountId, string badgeName)
        {
            var account = await _context.Accounts.Include(a => a.Badgenames).FirstOrDefaultAsync(a => a.Id == accountId);
            var badge = await _context.Badges.FindAsync(badgeName);

            if (account == null || badge == null)
                return NotFound();

            if (account.Badgenames.Contains(badge))
                return RedirectToAction("UsersBadges");

            account.Badgenames.Add(badge);
            await _context.SaveChangesAsync();

            return RedirectToAction("UsersBadges");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBadge(int accountId, string badgeName)
        {
            var account = await _context.Accounts
                .Include(a => a.Badgenames)
                .FirstOrDefaultAsync(a => a.Id == accountId);
            var badge = await _context.Badges.FindAsync(badgeName);

            if (account == null || badge == null)
                return NotFound();

            account.Badgenames.Remove(badge);
            await _context.SaveChangesAsync();

            return RedirectToAction("UsersBadges");
        }
    }
}
