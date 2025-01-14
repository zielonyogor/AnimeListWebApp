using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudioController : ControllerBase
    {
        private readonly ModelContext _context;

        public StudioController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Studio?search=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetStudios([FromQuery] string? search)
        {
            var studios = await _context.Studios
                .Where(s => string.IsNullOrEmpty(search) || s.Name.ToLower().StartsWith(search.ToLower()))
                .Select(s => new
                {
                    s.Name,
                    s.Wikipedialink,
                    Animes = s.Animes.Select(a => a.Mediumid).ToList()
                })
                .ToListAsync();

            return Ok(studios);
        }

        // GET: api/Studio/Mappa
        [HttpGet("{name}")]
        public async Task<ActionResult<StudioViewModel>> GetStudio(string name)
        {
            var studio = await _context.Studios
               .Select(s => new StudioViewModel
               {
                   Name = s.Name,
                   Wikipedialink = s.Wikipedialink,
                   AnimeIds = s.Animes.Select(a => a.Mediumid).ToList()
               })
               .FirstOrDefaultAsync(s => s.Name == name);

            if (studio == null)
            {
                return NotFound();
            }

            return studio;
        }

        // PUT: api/Studio/Mappa
        [HttpPut("{name}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> PutStudio(string name, [FromBody] Studio studio)
        {
            Console.WriteLine(name);
            if (name != studio.Name)
            {
                return BadRequest();
            }

            _context.Entry(studio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudioExists(name))
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

        // POST: api/Studio
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<Studio>> PostStudio([FromBody] Studio studio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine("aaaaaaaa\n\n\n\n\n");
            _context.Studios.Add(studio);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudioExists(studio.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudio", new { name = studio.Name }, studio);
        }

        // DELETE: api/Studio/Mappa
        [HttpDelete("{name}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteStudio(string name)
        {
            var studio = await _context.Studios.FindAsync(name);
            if (studio == null)
            {
                return NotFound();
            }

            _context.Studios.Remove(studio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudioExists(string name)
        {
            return _context.Studios.Any(e => e.Name == name);
        }
    }
}
