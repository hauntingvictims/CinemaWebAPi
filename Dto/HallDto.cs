using CinemaWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaWebApi.Dto;
public class HallDto
{
    [Required(ErrorMessage = "Hall number is required.")]
    public int HallNumber { get; set; }

    [Required(ErrorMessage = "Number of seats is required.")]
    public int NumberOfSeats { get; set; }

    [Required(ErrorMessage = "Hall type is required.")]
    public HallType Type { get; set; }
}