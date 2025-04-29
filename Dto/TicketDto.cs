using CinemaWebApi.Models; 
using System.ComponentModel.DataAnnotations;

namespace CinemaWebApi.Models
{
    public class TicketDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Showing ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Showing ID must be a positive.")]
        public int ShowingId { get; set; }

        [Required(ErrorMessage = "Seat number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seat number must be a positive.")]
        public int SeatNumber { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public TicketStatus Status { get; set; }
    }
}