using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/Anime
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes()
        {
            return await _context.Animes.ToListAsync();
        }

        // GET: api/Anime/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Anime>> GetAnime(int id)
        {
            var anime = await _context.Animes.FindAsync(id);

            if (anime == null)
            {
                return NotFound();
            }

            return anime;
        }

        // PUT: api/Anime/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnime(int id, Anime anime)
        {
            if (id != anime.Mediumid)
            {
                return BadRequest();
            }

            _context.Entry(anime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimeExists(id))
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

        // POST: api/Anime
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<Anime>> PostAnime(AnimeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var medium = new Medium
                {
                    Name = model.Name,
                    Status = model.Status,
                    Count = model.Count,
                    Poster = model.Poster,
                    Publishdate = model.Publishdate,
                    Description = model.Description,
                    Type = "A",
                    Genrenames = model.Genrenames
                };

                _context.Media.Add(medium);
                await _context.SaveChangesAsync();

                Console.WriteLine($"\n\nMedium ID: {medium.Id}");

                var anime = new Anime
                {
                    Mediumid = medium.Id,
                    Type = model.Type,
                    Studioname = model.Studioname
                };

                _context.Animes.Add(anime);

                foreach (var connectedMedium in model.Connections)
                {
                    var mediumConnection = await _context.Media.FindAsync(connectedMedium.Id);
                    if (mediumConnection != null)
                    {
                        medium.Idmedium1s.Add(mediumConnection);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Message = "Anime added successfully", MediumId = medium.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
            }
        }

        // DELETE: api/Anime/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnime(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime == null)
            {
                return NotFound();
            }

            _context.Animes.Remove(anime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimeExists(int id)
        {
            return _context.Animes.Any(e => e.Mediumid == id);
        }
    }
}
