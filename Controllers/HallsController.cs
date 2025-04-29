using CinemaWebApi.Data;
using CinemaWebApi.Models;
using CinemaWebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public HallsController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all halls")]
        [SwaggerResponse(200, "List of halls", typeof(List<Hall>))]
        public async Task<IActionResult> GetAll()
        {
            var halls = await _context.Halls.ToListAsync();
            return Ok(halls);
        }

        [HttpGet("{hallNumber}")]
        [SwaggerOperation(Summary = "Get hall by hall number")]
        [SwaggerResponse(200, "Hall found", typeof(Hall))]
        [SwaggerResponse(404, "Hall not found")]
        public async Task<IActionResult> GetById(int hallNumber)
        {
            var hall = await _context.Halls.FindAsync(hallNumber);
            if (hall == null)
            {
                return NotFound();
            }
            return Ok(hall);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new hall")]
        [SwaggerResponse(201, "Hall created ", typeof(Hall))]
        [SwaggerResponse(400, "Invalid hall data")]
        public async Task<IActionResult> Create([FromBody] HallDto hallDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (await _context.Halls.AnyAsync(h => h.HallNumber == hallDto.HallNumber))
            {
                return BadRequest("Hall with this hall number already exists.");
            }

            var hall = new Hall
            {
                HallNumber = hallDto.HallNumber,
                NumberOfSeat = hallDto.NumberOfSeats,
                HallType = hallDto.Type
            };

            await _context.Halls.AddAsync(hall);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { hallNumber = hall.HallNumber }, hall);
        }

        [HttpPut("{hallNumber}")]
        [SwaggerOperation(Summary = "Update existing hall")]
        [SwaggerResponse(204, "Hall updated")]
        [SwaggerResponse(400, "Invalid hall data")]
        [SwaggerResponse(404, "Hall not found")]
        public async Task<IActionResult> Update(int hallNumber, [FromBody] HallDto hallDto)
        {
            if (hallNumber != hallDto.HallNumber)
            {
                return BadRequest("Hall number in URL doesnt match the hall number in the body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingHall = await _context.Halls.FindAsync(hallNumber);
            if (existingHall == null)
            {
                return NotFound();
            }

            existingHall.NumberOfSeat = hallDto.NumberOfSeats;
            existingHall.HallType = hallDto.Type;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{hallNumber}")]
        [SwaggerOperation(Summary = "Delete hall")]
        [SwaggerResponse(204, "Hall deleted")]
        [SwaggerResponse(404, "Hall not found")]
        public async Task<IActionResult> Delete(int hallNumber)
        {
            var hall = await _context.Halls.FindAsync(hallNumber);
            if (hall == null)
            {
                return NotFound();
            }

            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}