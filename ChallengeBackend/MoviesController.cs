using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ChallengeBackend
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(string title = null, int? idGenre = null, string order = null)
        {
            var movies =_context.Movies
                .Where(x =>
                (string.IsNullOrEmpty(title) || x.Title.Contains(title)) &&
                (!idGenre.HasValue || x.Genre.Id == idGenre))
                .Select(x => new { x.Photo, x.Title, x.CreationDate });

            if (string.IsNullOrEmpty(order))
            {
                return Ok(movies);
            }

            if (order.ToLower() == "asc")
            {
                return Ok(movies.OrderBy(x => x.CreationDate));
            }

            if (order.ToLower() == "desc")
            {
                return Ok(movies.OrderByDescending(x => x.CreationDate));
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_context.Movies.Find(id));
        }

        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            _context.Movies.Add(movie);

            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Movie updatedMovie)
        {
            var movie = _context.Movies.Find(id);

            movie.Characters = updatedMovie.Characters;
            movie.CreationDate = updatedMovie.CreationDate;
            movie.Photo = updatedMovie.Photo;
            movie.Rate = updatedMovie.Rate;
            movie.Title = updatedMovie.Title;

            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Movies.Remove(_context.Movies.Find(id));

            _context.SaveChanges();
        }
    }
}
