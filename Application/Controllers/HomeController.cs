using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Models;

namespace Application.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly ModelContext _context;

    public HomeController(ILogger<HomeController> logger, ModelContext context) // 
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<Anime> animeList = _context.Animes.ToList();
        Console.WriteLine(animeList.Count);

        foreach (Anime anime in animeList)
        {
            Console.WriteLine(anime.Studioname);
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult UserInfo()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
