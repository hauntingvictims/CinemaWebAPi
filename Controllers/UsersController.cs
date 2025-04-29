using CinemaWebApi.Data;
using CinemaWebApi.Models;
using CinemaWebApi.Dto;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CinemaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public UsersController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all users")]
        [SwaggerResponse(200, "List of users", typeof(List<User>))]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by ID")]
        [SwaggerResponse(200, "User found", typeof(User))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new user")]
        [SwaggerResponse(201, "User created ", typeof(User))]
        [SwaggerResponse(400, "Invalid user data")]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                UserType = userDto.Type,
                Bonuses = userDto.Bonuses
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update existing user")]
        [SwaggerResponse(204, "User updated ")]
        [SwaggerResponse(400, "Invalid user data")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("ID in the URL doesnt match ID in the body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Перевірка унікальності email (якщо email змінено)
            if (existingUser.Email != userDto.Email && await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.UserType = userDto.Type;
            existingUser.Bonuses = userDto.Bonuses;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete user")]
        [SwaggerResponse(204, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}