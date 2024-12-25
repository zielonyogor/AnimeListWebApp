using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        // GET: api/<MangaController>
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MangaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MangaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MangaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MangaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
