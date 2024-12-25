using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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
        List<Anime> animeList = _context.Animes.ToList();
        Console.WriteLine(animeList.Count);

        foreach (Anime anime in animeList)
        {
            Console.WriteLine(anime.Studioname);
        }

        if (_signInManager.IsSignedIn(User))
        {
            Console.WriteLine("user is signed in");
            var user = _userManager.GetUserAsync(User).Result;
            ViewData["UserName"] = user.UserName;
            ViewData["Email"] = user.Email;
            ViewData["Description"] = user.Description;
            ViewData["ImageLink"] = user.Imagelink;
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
