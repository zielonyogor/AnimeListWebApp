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
    public class CharacterController : ControllerBase
    {
        private readonly ModelContext _context;

        public CharacterController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Character
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterViewModel>>> GetCharacters([FromQuery] string? search)
        {
            var characters = await _context.Characters
            .Include(c => c.Media)
            .Where(c => string.IsNullOrEmpty(search) || c.Name.ToLower().StartsWith(search.ToLower()))
            .Select(c => new CharacterViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
                Description = c.Description,
                Connections = c.Media.Select(m => m.Id).ToList()
            })
            .ToListAsync();

            return Ok(characters);
        }

        // GET: api/Character/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterViewModel>> GetCharacter(long id)
        {
            var character = await _context.Characters
                .Include(a => a.Media)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            var characterModel = new CharacterViewModel
            {
                Id = character.Id,
                Name = character.Name,
                Image = character.Image,
                Description = character.Description,
                Connections = character.Media.Select(m => m.Id).ToList()
            };

            return Ok(characterModel);
        }

        // PUT: api/Character/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> PutCharacter(long id, Character character)
        {
            if (id != character.Id)
            {
                return BadRequest("Ids are not matching");
            }

            var databaseCharacter = await _context.Characters.FindAsync(id);
            if (databaseCharacter == null)
            {
                return NotFound("Author not found");
            }

            databaseCharacter.Name = character.Name;
            databaseCharacter.Description = character.Description;

            if (!string.IsNullOrWhiteSpace(character.Image) && character.Image != databaseCharacter.Image)
            {
                Helper.DeleteImage(databaseCharacter.Image);

                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                var filename = $"character_{character.Id}_{Helper.RemoveWhitespace(character.Name)}{Path.GetExtension(character.Image)}";
                var filePath = Path.Combine(wwwrootPath, filename);

                int result = await Helper.SaveImage(character.Image, filePath);
                if (result == -1)
                {
                    return BadRequest("Could not save image");
                }

                var imagePath = $"/images/{filename}";
                databaseCharacter.Image = imagePath;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
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

        // POST: api/Character
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<CharacterViewModel>> PostCharacter(CharacterViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest("Invalid data");
            }

            var media = await _context.Media
                    .Where(m => model.Connections.Contains(m.Id))
                    .ToListAsync();

            var character = new Character
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Media = media
            };

            if (!string.IsNullOrWhiteSpace(model.Image))
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                var filename = $"character_{model.Id}_{Helper.RemoveWhitespace(model.Name)}{Path.GetExtension(model.Image)}";
                var filePath = Path.Combine(wwwrootPath, filename);

                int result = await Helper.SaveImage(model.Image, filePath);
                if (result == -1)
                {
                    return BadRequest("Could not save image");
                }

                var imagePath = $"/images/{filename}";
                model.Image = imagePath;
                character.Image = imagePath;
            }

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            model.Id = character.Id;

            return CreatedAtAction("GetCharacter", new { id = model.Id }, model);
        }

        // DELETE: api/Character/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteCharacter(long id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            // Deletes also image
            Helper.DeleteImage(character.Image);

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacterExists(long id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
