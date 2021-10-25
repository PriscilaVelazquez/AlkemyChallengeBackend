using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ChallengeBackend
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CharactersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(string name = null, short? age = null, short? weight = null, int? idMovie = null)
        {
            return Ok(_context.Characters
                .Where(x =>
                (string.IsNullOrEmpty(name) || x.Name.Contains(name)) &&
                (!age.HasValue || x.Age == age) &&
                (!weight.HasValue || x.Weight == weight) &&
                (!idMovie.HasValue || x.Movies.Any(f => f.Id == idMovie)))
                .Select(x => new { x.Photo, x.Name }));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_context.Characters.Find(id));
        }

        [HttpPost]
        public void Post([FromBody] Character character)
        {
            _context.Characters.Add(character);

            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Character updatedCharacter)
        {
            var character = _context.Characters.Find(id);

            character.Age = updatedCharacter.Age;
            character.Lore = updatedCharacter.Lore;
            character.Name = updatedCharacter.Name;
            character.Photo = updatedCharacter.Photo;
            character.Weight = updatedCharacter.Weight;
            character.Movies = updatedCharacter.Movies;

            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Characters.Remove(_context.Characters.Find(id));

            _context.SaveChanges();
        }
    }
}
