using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Application.Data;

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

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

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

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
	public async Task<IActionResult> Register(UserViewModel model, IFormFile? profilePicture)
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
            Description = model.Description,
            Accountprivilege = "n"
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
