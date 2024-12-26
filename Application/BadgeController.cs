using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;

namespace Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgeController : ControllerBase
    {
        private readonly ModelContext _context;

        public BadgeController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Badge
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Badge>>> GetBadges()
        {
            return await _context.Badges.ToListAsync();
        }

        // GET: api/Badge/beginner
        [HttpGet("{name}")]
        public async Task<ActionResult<Badge>> GetBadge(string name)
        {
            var badge = await _context.Badges.FindAsync(name);

            if (badge == null)
            {
                return NotFound();
            }

            return badge;
        }

        // PUT: api/Badge/beginner
        [HttpPut("{name}")]
        public async Task<IActionResult> PutBadge(string name, Badge badge)
        {
            if (name != badge.Name)
            {
                return BadRequest();
            }

            _context.Entry(badge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BadgeExists(name))
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

        // POST: api/Badge
        [HttpPost]
        public async Task<ActionResult<Badge>> PostBadge(Badge badge)
        {
            _context.Badges.Add(badge);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BadgeExists(badge.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBadge", new { name = badge.Name }, badge);
        }

        // DELETE: api/Badge/beginner
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteBadge(string name)
        {
            var badge = await _context.Badges.FindAsync(name);
            if (badge == null)
            {
                return NotFound();
            }

            _context.Badges.Remove(badge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BadgeExists(string name)
        {
            return _context.Badges.Any(e => e.Name == name);
        }
    }
}
