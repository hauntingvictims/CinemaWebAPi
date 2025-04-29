namespace CinemaWebApi.Dto;
using System.ComponentModel.DataAnnotations;

public class SaleDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Total amount is required.")]
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Purchase date is required.")]
    public DateTime PurchaseDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Bonuses used cannot be negative.")]
    public decimal BonusesUsed { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Bonuses earned cannot be negative.")]
    public decimal BonusesEarned { get; set; }
}