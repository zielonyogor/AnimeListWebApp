using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Data;
using Microsoft.AspNetCore.Identity;

namespace Application.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly ModelContext _context;

    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;

    public HomeController(ILogger<HomeController> logger, ModelContext context, SignInManager<Account> signInManager, UserManager<Account> userManager) // 
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        if (_signInManager.IsSignedIn(User))
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewData["UserName"] = user.UserName;
            return View();
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
