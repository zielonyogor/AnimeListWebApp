using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controllers;
public class ExploreController : Controller
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly ModelContext _context;

	private readonly SignInManager<Account> _signInManager;
	private readonly UserManager<Account> _userManager;

	public ExploreController(IHttpClientFactory httpClientFactory, ModelContext context, SignInManager<Account> signInManager, UserManager<Account> userManager)
	{
		_httpClientFactory = httpClientFactory;
		_context = context;
		_signInManager = signInManager;
		_userManager = userManager;
	}
	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Anime()
	{
		return View();
	}

	public async Task<IActionResult> AnimeDetails(int id)
	{
		var anime = await _context.Animes
		.Include(a => a.Medium)
		.ThenInclude(m => m.Genrenames)
		.Where(a => a.Mediumid == id)
		.Select(a => new AnimeViewModel
		{
			Id = a.Medium.Id,
			Name = a.Medium.Name,
			Poster = a.Medium.Poster,
			Status = a.Medium.Status,
			Publishdate = a.Medium.Publishdate,
			Type = a.Medium.Type,
			Studioname = a.Studioname,
			Description = a.Medium.Description,
			Genrenames = a.Medium.Genrenames.Select(g => g.Name).ToList(),
			Connections = a.Medium.Idmedium1s.Select(m => m.Id)
				.Union(a.Medium.Idmedium2s.Select(m => m.Id))
				.ToList(),
			Reviews = a.Medium.Reviews.Select(r => new ReviewViewModel
			{
				UserName = r.Account.UserName,
				Feeling = r.Feeling,
				Description = r.Description,
				Postdate = r.Postdate
			}).ToList()
		})
		.FirstOrDefaultAsync();

		if (anime == null)
			return NotFound();

		return View(anime);
	}

	public IActionResult Manga()
    {
        return View();
    }

	public async Task<IActionResult> MangaDetails(int id)
	{
		var manga = await _context.Mangas
		.Include(a => a.Medium)
		.ThenInclude(m => m.Genrenames)
		.Where(a => a.Mediumid == id)
		.Select(a => new MangaViewModel
		{
			Id = a.Medium.Id,
			Name = a.Medium.Name,
			Poster = a.Medium.Poster,
			Status = a.Medium.Status,
			Publishdate = a.Medium.Publishdate,
			Type = a.Medium.Type,
			AuthorId = a.Authorid,
			Description = a.Medium.Description,
			Genrenames = a.Medium.Genrenames.Select(g => g.Name).ToList(),
			Connections = a.Medium.Idmedium1s.Select(m => m.Id)
				.Union(a.Medium.Idmedium2s.Select(m => m.Id))
				.ToList(),
			Reviews = a.Medium.Reviews.Select(r => new ReviewViewModel
			{
				UserName = r.Account.UserName,
				Feeling = r.Feeling,
				Description = r.Description,
				Postdate = r.Postdate
			}).ToList()
		})
		.FirstOrDefaultAsync();

		if (manga == null)
			return NotFound();


		var author = await _context.Authors
			.FirstOrDefaultAsync(a => a.Id == manga.AuthorId);
		if (author != null)
			manga.AuthorName = author.Name;

		return View(manga);
	}

	public IActionResult Character()
    {
        return View();
    }

	public async Task<IActionResult> CharacterDetails(int id)
	{
		if (!_signInManager.IsSignedIn(User))
		{
			return RedirectToAction("Login", "Account");
		}

		var user = _userManager.GetUserAsync(User).Result;
		int accountId = user.Id;

		var character = await _context.Characters
			.Include(c => c.Media)
			.Include(c => c.Accounts)
			.Where(c => c.Id == id)
			.FirstOrDefaultAsync();

		if (character == null)
			return RedirectToAction("Character");

		bool isInFavorites = character?.Accounts.Any(a => a.Id == accountId) ?? false;
		ViewBag.IsInFavorites = isInFavorites;

		return View(character);
	}

	[HttpPost]
	public async Task<IActionResult> AddToFavorites(int characterId)
	{
		if (!_signInManager.IsSignedIn(User))
		{
			return RedirectToAction("Login", "Account");
		}

		var character = await _context.Characters
			.Include(c => c.Accounts)
			.FirstOrDefaultAsync(c => c.Id == characterId);

		if (character == null)
		{
			return NotFound();
		}

		var user = _userManager.GetUserAsync(User).Result;
		int accountId = user.Id;

		if (user != null)
		{
			if (!user.Characters.Contains(character))
			{
				user.Characters.Add(character);
				await _context.SaveChangesAsync();
			}
		}

		return RedirectToAction("Characters");
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> AddReview(int mediumId, string? returnUrl)
	{
		var medium = await _context.Media.FirstOrDefaultAsync(m => m.Id == mediumId);
		if (medium == null)
		{
			return NotFound();
		}

		var model = new ReviewViewModel
		{
			MediumId = mediumId,
			Name = medium.Name,
			Type = medium.Type == "A" ? "Anime" : "Manga",
			PublishDate = medium.Publishdate,
			ReturnUrl = returnUrl
		};

		return View(model);
	}

    [HttpPost]
    [Authorize]
	public async Task<IActionResult> SubmitReview(ReviewViewModel model)
    {
		model.Postdate = DateTime.Now;
		Console.WriteLine($"{model.Name} - {model.MediumId} : {model.Postdate}");
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            var review = new Review
            {
                Accountid = user.Id,
                Mediumid = model.MediumId,
                Description = model.Description!,
                Feeling = model.Feeling!,
                Postdate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

			if (!string.IsNullOrEmpty(model.ReturnUrl))
			{
				return Redirect(model.ReturnUrl);
			}

			return RedirectToAction("Explore");
		}

		return View("AddReview", model);
    }

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> Users()
	{
		var model = await _context.Accounts
			.Select(u => new UserInfoViewModel
			{
				UserName = u.UserName,
				Description = u.Description,
				Imagelink = u.Imagelink
			}).ToListAsync();
		return View(model);
	}
}