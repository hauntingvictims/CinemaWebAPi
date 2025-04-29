namespace CinemaWebApi.Models;

public class Ticket
{
    public int Id { get; set; }
    public int ShowingId { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public TicketStatus Status { get; set; }
    public int? SaleId { get; set; } 
    public Showing Showing { get; set; }
    public Sale Sale { get; set; }
}