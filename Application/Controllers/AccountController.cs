using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers;
public class AccountController : Controller
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;

    private readonly ModelContext _context;

    public AccountController(SignInManager<Account> signInManager, UserManager<Account> userManager, ModelContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    // GET: account?username=username123
    [HttpGet]
	[Authorize]
	public async Task<IActionResult> Index(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user != null)
        {
            var model = new UserInfoViewModel   
            {
                UserName = username,
                Imagelink = user.Imagelink,
                Description = user.Description,
                Createdate = String.Format("{0:dd/MM/yyyy}", user.Createdate),
            };
            ViewData["Title"] = $"{username} info";
            return View(model);
        }
        return RedirectToAction("Index", "Home");
    }

    // GET: account/edit
    [Authorize]
	[HttpGet]
	public async Task<IActionResult> Edit()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return RedirectToAction("Login", "Account");
		}

		var model = new UserEditViewModel
		{
			UserName = user.UserName,
			Email = user.Email,
			Imagelink = user.Imagelink,
			Description = user.Description
		};

		return View(model);
	}

    // POST: account/edit
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(UserEditViewModel model, IFormFile? profilePicture)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
        if (!isPasswordValid)
        {
            ModelState.AddModelError("CurrentPassword", "The current password is incorrect.");
            return View(model);
        }

        bool changesMade = false;

        if (model.UserName != user.UserName)
        {
            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "This username is already taken.");
                return View(model);
            }

            user.UserName = model.UserName;
            changesMade = true;
        }

        if (model.Description != user.Description)
        {
            user.Description = model.Description;
            changesMade = true;
        }
        if (profilePicture != null)
        {
            var filePath = Path.Combine("wwwroot/images", profilePicture.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }
            user.Imagelink = $"/images/{profilePicture.FileName}";
            changesMade = true;
        }


        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            var passwordValidator = new PasswordValidator<Account>();
            var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

            if (!passwordValidationResult.Succeeded)
            {
                foreach (var error in passwordValidationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            var updatePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
            if (!updatePasswordResult.Succeeded)
            {
                foreach (var error in updatePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            changesMade = true;
        }


        if (!changesMade)
        {
            return RedirectToAction(nameof(Edit));
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (updateResult.Succeeded)
        {
            TempData["SuccessMessage"] = "Profile has updated";
            return RedirectToAction(nameof(Edit));
        }

        foreach (var error in updateResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }


    // GET: account/login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    // POST: account/login
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.EmailOrUsername)
                    ?? await _userManager.FindByNameAsync(model.EmailOrUsername);

        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    // GET: account/register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: account/register
    [HttpPost]
	public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? profilePicture)
	{
        if (!ModelState.IsValid)
            return View(model);

        string imageUrl = string.Empty;
        if (profilePicture != null)
        {
            var filePath = Path.Combine("wwwroot/images", profilePicture.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }
            imageUrl = $"/images/{profilePicture.FileName}";
        }

        var user = new Account
        {
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = _userManager.PasswordHasher.HashPassword(null!, model.Password),
            Createdate = DateTime.UtcNow,
            Imagelink = imageUrl,
            Description = model.Description
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // POST: account/logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied(string ReturnUrl)
    {
        return View();
    }

    // GET: account/isUsernameAvailable?username=username123 - like this just to have this in /account/
    [HttpGet]
    public async Task<IActionResult> IsUsernameAvailable(string username)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
        {
            return Json(new { isValid = false, message = "Username must be at least 3 characters long." });
        }

        var count = await _context.Users
            .Where(u => u.UserName == username)
            .CountAsync();
        bool userExists = count > 0;

        if (userExists)
        {
            return Json(new { isValid = false, message = "Username is already taken." });
        }

        return Json(new { isValid = true, message = "Username is available." });
    }
}