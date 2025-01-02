using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
		var client = _httpClientFactory.CreateClient("Application");
		var response = await client.GetAsync($"/api/Anime/{id}");
		if (response.IsSuccessStatusCode)
		{
			var anime = await response.Content.ReadFromJsonAsync<AnimeViewModel>();
			if (anime == null)
				return RedirectToAction("Anime");

			return View(anime);
		}
		return RedirectToAction("Anime");
	}

	public IActionResult Manga()
    {
        return View();
    }

	public async Task<IActionResult> MangaDetails(int id)
	{
		var client = _httpClientFactory.CreateClient("Application");
		var response = await client.GetAsync($"/api/Manga/{id}");
		if (response.IsSuccessStatusCode)
		{
			var manga = await response.Content.ReadFromJsonAsync<MangaViewModel>();
			if (manga == null)
				return RedirectToAction("Manga");

			Console.WriteLine(manga.Connections.Count);

			return View(manga);
		}
		return RedirectToAction("Manga");
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
		//var client = _httpClientFactory.CreateClient("Application");
		//var response = await client.GetAsync($"/api/Character/{id}");
		//if (response.IsSuccessStatusCode)
		//{
		//	var character = await response.Content.ReadFromJsonAsync<CharacterViewModel>();
		//	if (character == null)
		//		return RedirectToAction("Character");

		//	Character characterModel = new Character { 
		//		Id = character.Id,
		//		Name = character.Name,
		//		Description = character.Description,
		//		Image = character.Image
		//	};
		//	if (character.Connections.Count != 0)
		//	{
		//		foreach (var conn in character.Connections)
		//		{
		//			var responseConn = await client.GetAsync($"/api/Media/{conn}");
		//			if (responseConn.IsSuccessStatusCode)
		//			{
		//				var medium = await responseConn.Content.ReadFromJsonAsync<Medium>();
		//				if (medium == null)
		//					continue;
		//				characterModel.Media.Add(medium);
		//			}
		//		}
		//	}

		//	bool isFavorite = 

		//	return View(characterModel);
		//}
		//return RedirectToAction("Character");
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
}