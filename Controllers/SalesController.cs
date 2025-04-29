using CinemaWebApi.Data;
using CinemaWebApi.Dto;
using CinemaWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CinemaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public SalesController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all sales")]
        [SwaggerResponse(200, "List of sales", typeof(List<Sale>))]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _context.Sales.ToListAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get sale by ID")]
        [SwaggerResponse(200, "Sale found", typeof(Sale))]
        [SwaggerResponse(404, "Sale not found")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new sale")]
        [SwaggerResponse(201, "Sale created", typeof(Sale))]
        [SwaggerResponse(400, "Invalid sale data")]
        public async Task<IActionResult> Create([FromBody] SaleDto saleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

       
            var user = await _context.Users.FindAsync(saleDto.UserId);
            if (user == null)
            {
                return BadRequest("User doesnt exist.");
            }

            var sale = new Sale
            {
                UserId = saleDto.UserId,
                TotalAmount = saleDto.TotalAmount,
                PurchaseDate = saleDto.PurchaseDate,
                BonusesUsed = saleDto.BonusesUsed,
                BonusesEarned = saleDto.BonusesEarned
            };

            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update existing sale")]
        [SwaggerResponse(204, "Sale updated ")]
        [SwaggerResponse(400, "Invalid sale data")]
        [SwaggerResponse(404, "Sale not found")]
        public async Task<IActionResult> Update(int id, [FromBody] SaleDto saleDto)
        {
            if (id != saleDto.Id)
            {
                return BadRequest("ID in the URL doesnt match ID in the body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var user = await _context.Users.FindAsync(saleDto.UserId);
            if (user == null)
            {
                return BadRequest("User doesnt exist.");
            }

            var existingSale = await _context.Sales.FindAsync(id);
            if (existingSale == null)
            {
                return NotFound();
            }

            existingSale.UserId = saleDto.UserId;
            existingSale.TotalAmount = saleDto.TotalAmount;
            existingSale.PurchaseDate = saleDto.PurchaseDate;
            existingSale.BonusesUsed = saleDto.BonusesUsed;
            existingSale.BonusesEarned = saleDto.BonusesEarned;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete sale")]
        [SwaggerResponse(204, "Sale deleted")]
        [SwaggerResponse(404, "Sale not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}