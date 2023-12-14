using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Booking;

public class CreateBookingDto
{
    
    [Range(1, long.MaxValue, ErrorMessage = "Time must be greater than 0")]
    public long Time { get; set; }

    [Range(0, 2, ErrorMessage = "Status must be between 0 and 2")]
    public int Status { get; set; }

    [Required(ErrorMessage = "ServiceId is required")]
    public Guid ServiceId { get; set; }

    [Required(ErrorMessage = "ClientId is required")]
    public Guid ClientId { get; set; }
}