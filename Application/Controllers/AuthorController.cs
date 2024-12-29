using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Application.Misc;
using System.Xml.Linq;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ModelContext _context;

        public AuthorController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Author?search=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAuthors([FromQuery] string? search)
        {
            var authors = await _context.Authors
                .Where(a => string.IsNullOrEmpty(search) || a.Name.ToLower().Contains(search.ToLower()))
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Image,
                    a.Wikipedialink,
                    Manga = a.Mangas.Select(m => m.Mediumid).ToList()
                })
                .ToListAsync();

            return Ok(authors);
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAuthor(short id)
        {
            var author = await _context.Authors
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Image,
                    a.Wikipedialink,
                    Manga = a.Mangas.Select(m => m.Mediumid).ToList()
                })
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> PutAuthor(short id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest("Ids are not matching");
            }

            var databaseAuthor = await _context.Authors.FindAsync(id);
            if (databaseAuthor == null)
            {
                return NotFound("Author not found");
            }

            databaseAuthor.Name = author.Name;
            databaseAuthor.Wikipedialink = author.Wikipedialink;

            if (!String.IsNullOrWhiteSpace(author.Image) && author.Image != databaseAuthor.Image)
            {
                Helper.DeleteImage(databaseAuthor.Image);

                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                var filename = $"author_{author.Id}_{Helper.RemoveWhitespace(author.Name)}{Path.GetExtension(author.Image)}";
                var filePath = Path.Combine(wwwrootPath, filename);

                int result = await Helper.SaveImage(author.Image, filePath);
                if (result == -1)
                {
                    return BadRequest("Could not save image");
                }

                var imagePath = $"/images/{filename}";
                databaseAuthor.Image = imagePath;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Author
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (author == null || String.IsNullOrWhiteSpace(author.Name))
            {
                return BadRequest("Invalid data");
            }

            if(!String.IsNullOrWhiteSpace(author.Image))
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                var filename = $"author_{author.Id}_{Helper.RemoveWhitespace(author.Name)}{Path.GetExtension(author.Image)}";
                var filePath = Path.Combine(wwwrootPath, filename);

                int result = await Helper.SaveImage(author.Image, filePath);
                if (result == -1)
                { 
                    return BadRequest("Could not save image");
                }

                var imagePath = $"/images/{filename}";
                author.Image = imagePath;
            }


            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Author/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteAuthor(short id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            // Deletes also image
            Helper.DeleteImage(author.Image);

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(short id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
