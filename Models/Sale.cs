namespace CinemaWebApi.Models;

public class Sale
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal BonusesUsed { get; set; }
    public decimal BonusesEarned { get; set; }
    public User User { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}