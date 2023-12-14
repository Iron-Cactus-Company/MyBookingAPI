using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.Booking;

public class CreateBookingDto
{
    [Required(ErrorMessage = "Time is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Time must be greater than 0")]
    public long Time { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [Range(0, 2, ErrorMessage = "Status must be between 0 and 2")]
    public int Status { get; set; }

    [Required(ErrorMessage = "ServiceId is required")]
    [GuidValidation]
    public Guid ServiceId { get; set; }

    [Required(ErrorMessage = "ClientId is required")]
    [GuidValidation]
    public Guid ClientId { get; set; }
}