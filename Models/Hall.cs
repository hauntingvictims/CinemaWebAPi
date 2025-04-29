namespace CinemaWebApi.Models;

public class Hall
{
    public int HallNumber { get; set; }
    public int NumberOfSeat { get; set; }
    public HallType HallType { get; set; }
    public ICollection<Showing> Showings { get; set; }
}