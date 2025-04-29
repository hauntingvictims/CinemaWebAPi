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
    public class TicketsController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public TicketsController(CinemaDbContext context)
        {   
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all tickets")]
        [SwaggerResponse(200, "List of tickets", typeof(List<Ticket>))]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get ticket by ID")]
        [SwaggerResponse(200, "Ticket found", typeof(Ticket))]
        [SwaggerResponse(404, "Ticket not found")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new ticket")]
        [SwaggerResponse(201, "Ticket created", typeof(Ticket))]
        [SwaggerResponse(400, "Invalid ticket data")]
        public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var showing = await _context.Showings.FindAsync(ticketDto.ShowingId);
            if (showing == null)
            {
                return BadRequest("Showing doesnt exist.");
            }

            var ticket = new Ticket
            {
                ShowingId = ticketDto.ShowingId,
                SeatNumber = ticketDto.SeatNumber,
                Price = ticketDto.Price,
                Status = ticketDto.Status
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update existing ticket")]
        [SwaggerResponse(204, "Ticket updated")]
        [SwaggerResponse(400, "Invalid ticket data")]
        [SwaggerResponse(404, "Ticket not found")]
        public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
        {
            if (id != ticketDto.Id)
            {
                return BadRequest("ID in the URL doesnt match ID in the body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Перевірка існування сеансу
            var showing = await _context.Showings.FindAsync(ticketDto.ShowingId);
            if (showing == null)
            {
                return BadRequest("Showing does not exist.");
            }

            var existingTicket = await _context.Tickets.FindAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            existingTicket.ShowingId = ticketDto.ShowingId;
            existingTicket.SeatNumber = ticketDto.SeatNumber;
            existingTicket.Price = ticketDto.Price;
            existingTicket.Status = ticketDto.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete ticket")]
        [SwaggerResponse(204, "Ticket deleted")]
        [SwaggerResponse(404, "Ticket not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}