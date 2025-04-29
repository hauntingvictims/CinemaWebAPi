namespace CinemaWebApi.Dto;
using CinemaWebApi.Models;
using System.ComponentModel.DataAnnotations;

public class UserDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "User type is required.")]
    public UserType Type { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Bonuses cannot be negative.")]
    public decimal Bonuses { get; set; }
}