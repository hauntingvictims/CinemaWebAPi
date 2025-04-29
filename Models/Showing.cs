namespace CinemaWebApi.Models;

public class Showing
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int HallNumber { get; set; }
    public DateTime StartTime { get; set; }
    public decimal TicketPrice { get; set; }
    public ShowingStatus Status { get; set; }
    public Movie Movie { get; set; }
    public Hall Hall { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    
}