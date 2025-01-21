using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ModelContext _context;

        public MediaController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Media?search=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMedia([FromQuery] string? search)
        {
            var media = await _context.Media
                .Where(m => string.IsNullOrEmpty(search) || m.Name.ToLower().Contains(search.ToLower()))
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    Type = m.Type == "A" ? "Anime" : "Manga",
                    m.Publishdate,
                    m.Status,
                    m.Count
                })
                .ToListAsync();

            return Ok(media);
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMedium(int id)
        {
            var medium = await _context.Media
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    Type = m.Type == "A" ? "Anime" : "Manga",
                    m.Publishdate,
                    m.Poster,
                    m.Status,
                    m.Count
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medium == null)
            {
                return NotFound();
            }

            return Ok(medium);
        }
    }
}
