using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Application.Misc;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly ModelContext _context;

        public MangaController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Manga
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MangaViewModel>>> GetMangas()
        {
            var mangas = await _context.Mangas
            .Include(m => m.Medium)
            .ThenInclude(m => m.Genrenames)
            .Select(m => new MangaViewModel
            {
                Id = m.Mediumid,
                Name = m.Medium.Name,
                Status = m.Medium.Status,
                Count = m.Medium.Count,
                Poster = m.Medium.Poster,
                Publishdate = m.Medium.Publishdate,
                Description = m.Medium.Description,
                Type = m.Medium.Type,
                AuthorId = m.Authorid,
                Genrenames = m.Medium.Genrenames.Select(g => g.Name).ToList(),
                Connections = m.Medium.Idmedium1s.Select(m => m.Id).Union(m.Medium.Idmedium2s.Select(m => m.Id)).ToList()
            })
            .ToListAsync();

            return Ok(mangas);
        }

        // GET: api/Manga/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MangaViewModel>> GetManga(int id)
        {
            var manga = await _context.Mangas
                .Include(a => a.Medium)
                .ThenInclude(m => m.Genrenames)
                .FirstOrDefaultAsync(a => a.Mediumid == id);

            if (manga == null)
            {
                return NotFound();
            }

            var mangaModel = new MangaViewModel
            {
                Id = manga.Mediumid,
                Name = manga.Medium.Name,
                Status = manga.Medium.Status,
                Count = manga.Medium.Count,
                Poster = manga.Medium.Poster,
                Publishdate = manga.Medium.Publishdate,
                Description = manga.Medium.Description,
                Type = manga.Medium.Type,
                AuthorId = manga.Authorid,
                Genrenames = manga.Medium.Genrenames.Select(g => g.Name).ToList(),
                Connections = manga.Medium.Idmedium1s.Select(m => m.Id).Union(manga.Medium.Idmedium2s.Select(m => m.Id)).ToList()
            };

            return Ok(mangaModel);
        }

        // PUT: api/Manga/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> PutManga(int id, MangaViewModel model)
        {
            if (id != model.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var databaseMedium = await _context.Media
                    .Include(m => m.Idmedium1s)
                    .Include(m => m.Idmedium2s)
                    .Include(m => m.Genrenames)
                    .FirstOrDefaultAsync(m => m.Id == id);
                var databaseManga = await _context.Mangas.FindAsync(id);

                if (databaseMedium == null || databaseManga == null)
                {
                    return NotFound("Manga not found");
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

                if (!string.IsNullOrWhiteSpace(model.Poster) && model.Poster != databaseMedium.Poster)
                {
                    Helper.DeleteImage(databaseMedium.Poster);

                    var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    var filename = $"manga_{model.Id}_{Helper.RemoveWhitespace(model.Name)}{Path.GetExtension(model.Poster)}";
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

                databaseManga.Authorid = model.AuthorId;
                databaseManga.Type = model.Type;

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

        // POST: api/Manga
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<MangaViewModel>> PostManga(MangaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
                    Type = "M",
                    Genrenames = genres
                };

                _context.Media.Add(medium);
                await _context.SaveChangesAsync();

                Console.WriteLine($"\n\nMedium ID: {medium.Id}\n\n");
                model.Id = medium.Id;

                if (!string.IsNullOrWhiteSpace(model.Poster))
                {
                    var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    var filename = $"manga_{model.Id}_{Helper.RemoveWhitespace(model.Name)}{Path.GetExtension(model.Poster)}";
                    var filePath = Path.Combine(wwwrootPath, filename);

                    int result = await Helper.SaveImage(model.Poster, filePath);
                    if (result == -1)
                    {
                        return BadRequest("Could not save image");
                    }

                    var imagePath = $"/images/{filename}";
                    medium.Poster = imagePath;
                    model.Poster = imagePath;
                    await _context.SaveChangesAsync();
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

                var manga = new Manga
                {
                    Mediumid = medium.Id,
                    Type = model.Type,
                    Authorid = model.AuthorId
                };

                _context.Mangas.Add(manga);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // writing defaults
                model.Status = medium.Status;
                model.Publishdate = medium.Publishdate;
                model.Type = manga.Type;

                return CreatedAtAction("GetManga", new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

        // DELETE: api/Manga/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteManga(int id)
        {
            var databaseMedium = await _context.Media.FindAsync(id);
            var databaseManga = await _context.Mangas.FindAsync(id);

            if (databaseMedium == null || databaseManga == null)
            {
                return NotFound("Manga not found");
            }

            // Deletes also image
            Helper.DeleteImage(databaseMedium.Poster);

            _context.Mangas.Remove(databaseManga);
            _context.Media.Remove(databaseMedium);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
