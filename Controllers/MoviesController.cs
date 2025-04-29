using CinemaWebApi.Models;
using CinemaWebApi.Dto;
using CinemaWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApiCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all movies")]
        [SwaggerResponse(200, "List of movies", typeof(List<Movie>))]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get movie by ID")]
        [SwaggerResponse(200, "Movie found", typeof(Movie))]
        [SwaggerResponse(404, "Movie not found")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new movie")]
        [SwaggerResponse(201, "Movie created successfully", typeof(Movie))]
        [SwaggerResponse(400, "Invalid movie data")]
        public async Task<IActionResult> Create([FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = new Movie
            {
                Title = movieDto.Title,
                Genre = movieDto.Genre,
                Director = movieDto.Director,
                Duration = movieDto.Duration,
                ReleaseYear = movieDto.ReleaseYear,
                AgeRestriction = movieDto.AgeRestriction,
                Description = movieDto.Description
            };

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update existing movie")]
        [SwaggerResponse(204, "Movie updated successfully")]
        [SwaggerResponse(400, "Invalid movie data")]
        [SwaggerResponse(404, "Movie not found")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDto movieDto)
        {
            if (id != movieDto.Id)
            {
                return BadRequest("ID in the URL doesnt match ID in the body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            existingMovie.Title = movieDto.Title;
            existingMovie.Genre = movieDto.Genre;
            existingMovie.Director = movieDto.Director;
            existingMovie.Duration = movieDto.Duration;
            existingMovie.ReleaseYear = movieDto.ReleaseYear;
            existingMovie.AgeRestriction = movieDto.AgeRestriction;
            existingMovie.Description = movieDto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete movie")]
        [SwaggerResponse(204, "Movie deleted")]
        [SwaggerResponse(404, "Movie not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}