using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Application.Misc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        private readonly ModelContext _context;

        public AnimeController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Anime?search=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimeViewModel>>> GetAnimes([FromQuery] string? search)
        {
            Console.WriteLine(search);
            var animes = await _context.Animes
            .Include(a => a.Medium)
            .ThenInclude(m => m.Genrenames)
            .Where(a => string.IsNullOrEmpty(search) || a.Medium.Name.ToLower().StartsWith(search.ToLower()))
            .Select(a => new AnimeViewModel
            {
                Id = a.Mediumid,
                Name = a.Medium.Name,
                Status = a.Medium.Status,
                Count = a.Medium.Count,
                Poster = a.Medium.Poster,
                Publishdate = a.Medium.Publishdate,
                Description = a.Medium.Description,
                Type = a.Type,
                Studioname = a.Studioname,
                Genrenames = a.Medium.Genrenames.Select(g => g.Name).ToList(),
                Connections = a.Medium.Idmedium1s.Select(m => m.Id).Union(a.Medium.Idmedium2s.Select(m => m.Id)).ToList()
            })
            .ToListAsync();

            return Ok(animes);
        }

        // GET: api/Anime/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimeViewModel>> GetAnime(int id)
        {
            var anime = await _context.Animes
                .Include(a => a.Medium)
				.ThenInclude(m => m.Idmedium1s)
				.ThenInclude(m => m.Idmedium2s)
				.ThenInclude(m => m.Genrenames)
				.FirstOrDefaultAsync(a => a.Mediumid == id);

            if (anime == null)
            {
                return NotFound();
            }

            var animeModel = new AnimeViewModel
            {
                Id = anime.Mediumid,
                Name = anime.Medium.Name,
                Status = anime.Medium.Status,
                Count = anime.Medium.Count,
                Poster = anime.Medium.Poster,
                Publishdate = anime.Medium.Publishdate,
                Description = anime.Medium.Description,
                Type = anime.Type,
                Studioname = anime.Studioname,
                Genrenames = anime.Medium.Genrenames.Select(g => g.Name).ToList(),
                Connections = anime.Medium.Idmedium1s.Select(m => m.Id).Union(anime.Medium.Idmedium2s.Select(m => m.Id)).ToList()
            };

            return Ok(animeModel);
        }

        // PUT: api/Anime/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> PutAnime(int id, AnimeViewModel model)
        {
            if (id != model.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var errors = ValidationCheck(model);
            if (errors != null)
            {
                return BadRequest(errors);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var databaseMedium = await _context.Media
                    .Include(m => m.Idmedium1s)
                    .Include(m => m.Idmedium2s)
                    .Include(m => m.Genrenames)
                    .FirstOrDefaultAsync(m => m.Id == id);
                var databaseAnime = await _context.Animes.FindAsync(id);

                if (databaseMedium == null || databaseAnime == null)
                {
                    return NotFound("Anime not found");
                }

                var mediumConnections = await _context.Media
                    .Where(m => model.Connections.Contains(m.Id))
                    .ToListAsync();
                var removedConnections1 = databaseMedium.Idmedium1s
                    .Where(existing => !model.Connections.Contains(existing.Id))
                    .ToList();
                var removedConnections2 = databaseMedium.Idmedium2s
                    .Where(existing => !model.Connections.Contains(existing.Id))
                    .ToList();

                foreach (var removed in removedConnections1)
                {
                    databaseMedium.Idmedium1s.Remove(removed);
                }
                foreach (var removed in removedConnections2)
                {
                    databaseMedium.Idmedium2s.Remove(removed);
                }

                foreach (var connection in mediumConnections)
                {
                    if (!databaseMedium.Idmedium1s.Contains(connection))
                    {
                        databaseMedium.Idmedium1s.Add(connection);
                    }
                    if (!databaseMedium.Idmedium2s.Contains(connection))
                    {
                        databaseMedium.Idmedium2s.Add(connection);
                    }
                }

                var genres = await _context.Genres
                    .Where(g => model.Genrenames.Contains(g.Name))
                    .ToListAsync();
                var removedGenres = databaseMedium.Genrenames
                    .Where(existing => !model.Genrenames.Contains(existing.Name))
                    .ToList();

                foreach (var removed in removedGenres)
                {
                    databaseMedium.Genrenames.Remove(removed);
                }
                foreach (var genre in genres)
                {
                    if (!databaseMedium.Genrenames.Any(g => g.Name == genre.Name))
                    {
                        databaseMedium.Genrenames.Add(genre);
                    }
                }

                databaseMedium.Name = model.Name;
                databaseMedium.Status = model.Status;
                databaseMedium.Count = model.Count;
                databaseMedium.Publishdate = model.Publishdate;
                databaseMedium.Description = model.Description;

                if (!String.IsNullOrWhiteSpace(model.Poster) && model.Poster != databaseMedium.Poster)
                {
                    Helper.DeleteImage(databaseMedium.Poster);

                    var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    var filename = $"anime_{model.Id}_{Helper.RemoveWhitespace(model.Name)}{Path.GetExtension(model.Poster)}";
                    var filePath = Path.Combine(wwwrootPath, filename);

                    int result = await Helper.SaveImage(model.Poster, filePath);
                    if (result == -1)
                    {
                        return BadRequest("Could not save image");
                    }

                    var imagePath = $"/images/{filename}";
                    databaseMedium.Poster = imagePath;
                }

                await _context.SaveChangesAsync();

                var studio = await _context.Studios.FindAsync(model.Studioname);
                if(studio != databaseAnime.StudionameNavigation)
                {
                    databaseAnime.Studioname = model.Studioname;
                }
                databaseAnime.Type = model.Type;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

        // POST: api/Anime
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<AnimeViewModel>> PostAnime(AnimeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var errors = ValidationCheck(model);
            if (errors != null)
            {
                return BadRequest(errors);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var genres = await _context.Genres
                    .Where(g => model.Genrenames.Contains(g.Name))
                    .ToListAsync();

                var medium = new Medium
                {
                    Name = model.Name,
                    Status = model.Status,
                    Count = model.Count,
                    Poster = model.Poster,
                    Publishdate = model.Publishdate,
                    Description = model.Description,
                    Type = "A",
                    Genrenames = genres
                };

                _context.Media.Add(medium);
                await _context.SaveChangesAsync();

                Console.WriteLine($"\n\nMedium ID: {medium.Id}\n\n");
                model.Id = medium.Id;

                if (!String.IsNullOrWhiteSpace(model.Poster))
                {
                    var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    var filename = $"anime_{model.Id}_{Helper.RemoveWhitespace(model.Name)}{Path.GetExtension(model.Poster)}";
                    var filePath = Path.Combine(wwwrootPath, filename);

                    int result = await Helper.SaveImage(model.Poster, filePath);
                    if (result == -1)
                    {
                        return BadRequest("Could not save image");
                    }

                    var imagePath = $"/images/{filename}";
                    model.Poster = imagePath;
                }


                if (model.Connections != null && model.Connections.Any())
                {
                    var connections = await _context.Media
                        .Where(m => model.Connections.Contains(m.Id))
                        .ToListAsync();

                    foreach (var existingMedium in connections)
                    {
                        medium.Idmedium1s.Add(existingMedium);
                        existingMedium.Idmedium2s.Add(medium);
                    }
                    await _context.SaveChangesAsync();
                }

                var anime = new Anime
                {
                    Mediumid = medium.Id,
                    Type = model.Type,
                    Studioname = model.Studioname
                };

                _context.Animes.Add(anime);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // writing defaults
                model.Status = medium.Status;
                model.Publishdate = medium.Publishdate;
                model.Type = anime.Type;

                return CreatedAtAction("GetAnime", new { id = model.Id }, model );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

        // DELETE: api/Anime/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteAnime(int id)
        {
            var databaseMedium = await _context.Media.FindAsync(id);
            var databaseAnime = await _context.Animes.FindAsync(id);

            if (databaseMedium == null || databaseAnime == null)
            {
                return NotFound("Anime not found");
            }

            // Deletes also image
            Helper.DeleteImage(databaseMedium.Poster);

            _context.Animes.Remove(databaseAnime);
            _context.Media.Remove(databaseMedium);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private static ValidationProblemDetails? ValidationCheck(AnimeViewModel model)
        {
            ValidationProblemDetails result = new ValidationProblemDetails
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            };

            if (model.Status == "To be released")
            {
                if (model.Count != 0)
                {
                    result.Errors.Add("Count", ["Not published Anime can't have chapters"]);

                }
                if (model.Publishdate <= DateTime.Now)
                {
                    result.Errors.Add("Publishdate", ["Not published Anime must have future publish date"]);
                }
            }
            else if (model.Status == "Finished")
            {
                if (model.Count == 0)
                {
                    result.Errors.Add("Count", ["Finished Anime must have chapters"]);
                }
                if (model.Publishdate >= DateTime.Now)
                {
                    result.Errors.Add("Publishdate", ["Finished Anime can't have future publish date"]);
                }
            }
            else if (model.Status == "Not finished")
            {   
                if (model.Count == 0)
                {
                    result.Errors.Add("Count", ["Not finished Anime must have chapters"]);
                }
                if (model.Publishdate >= DateTime.Now)
                {
                    result.Errors.Add("Publishdate", ["Not finished Anime can't have future publish date"]);
                }
            }

            if ((model.Type == "Movie" || model.Type == "OVA") && model.Count != 0)
            {
                result.Errors.Add("Type", ["Movies/OVAs can only have one chapter"]);
            }

            if (result.Errors.Count == 0)
                return null;
            return result;
        }
    }
}
