namespace CinemaWebApi.Dto;
using System.ComponentModel.DataAnnotations;

public class MovieDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Genre is required.")]
    public string Genre { get; set; }

    [Required(ErrorMessage = "Director is required.")]
    public string Director { get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Release year is required.")]
    [Range(1990, 2025, ErrorMessage = "Release year must be between 1990 and 2025.")]
    public int ReleaseYear { get; set; }

    [Required(ErrorMessage = "Age restrictions are required.")] 
    public string AgeRestriction { get; set; }

    [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
    public string Description { get; set; }
}