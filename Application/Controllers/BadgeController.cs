using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;

namespace Application.Controllers
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
        public async Task<ActionResult<IEnumerable<object>>> GetBadges()
        {
            var badges = await _context.Badges
                .Select(b => new
                {
                    b.Name,
                    b.Namecolor,
                    b.Backgroundcolor,
                    b.Description,
                    Accounts = b.Accounts.Select(a => a.Id).ToList()
                })
                .ToListAsync();

            return Ok(badges);
        }

        // GET: api/Badge/beginner
        [HttpGet("{name}")]
        public async Task<ActionResult<object>> GetBadge(string name)
        {
            var badge = await _context.Badges
                .Select(b => new
                {
                    b.Name,
                    b.Namecolor,
                    b.Backgroundcolor,
                    b.Description,
                    Accounts = b.Accounts.Select(a => a.Id).ToList()
                })
                .FirstOrDefaultAsync(b => b.Name == name);

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
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

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
