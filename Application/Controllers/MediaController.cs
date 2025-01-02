using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;

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
                .Where(m => string.IsNullOrEmpty(search) || m.Name.ToLower().StartsWith(search.ToLower()))
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    Type = m.Type == "A" ? "Anime" : "Manga",
                    m.Publishdate
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
            Console.WriteLine($"{id}, {medium?.Name} \n\n\n");
            if (medium == null)
            {
                return NotFound();
            }

            return Ok(medium);
        }
    }
}
