using System.Text.Json.Serialization;

namespace CinemaWebApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }
    public decimal Bonuses { get; set; }
    [JsonIgnore]
    public ICollection<Sale> Sales { get; set; }
}