using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers
{
	public class ExploreController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ExploreController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
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
			var client = _httpClientFactory.CreateClient("Application");
			var response = await client.GetAsync($"/api/Character/{id}");
			if (response.IsSuccessStatusCode)
			{
				var character = await response.Content.ReadFromJsonAsync<CharacterViewModel>();
				if (character == null)
					return RedirectToAction("Character");

				Character characterModel = new Character { 
					Id = character.Id,
					Name = character.Name,
					Description = character.Description,
					Image = character.Image
				};
				if (character.Connections.Count != 0)
				{
					foreach (var conn in character.Connections)
					{
						var responseConn = await client.GetAsync($"/api/Media/{conn}");
						if (responseConn.IsSuccessStatusCode)
						{
							var medium = await responseConn.Content.ReadFromJsonAsync<Medium>();
							if (medium == null)
								continue;
							characterModel.Media.Add(medium);
						}
					}
				}

                foreach (var medium in characterModel.Media)
                {
					Console.WriteLine($"Poster path: {medium.Poster}");
                }

                return View(characterModel);
			}
			return RedirectToAction("Character");
		}
	}
}
