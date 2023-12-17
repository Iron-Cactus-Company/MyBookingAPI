using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.Booking;

public class UpdateBookingDto
{
    
    [Required(ErrorMessage = "Id is required")]
    [GuidValidation]
    public string Id { get; set; }

    [Required(ErrorMessage = "Time is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Time must be greater than 0")]
    public long Time { get; set; }
    
    [Range(0, 2, ErrorMessage = "Status must be between 0 and 2")]
    public int Status { get; set; }
    
    [GuidValidation]
    public string ServiceId { get; set; }

    [GuidValidation]
    public string ClientId { get; set; }
}