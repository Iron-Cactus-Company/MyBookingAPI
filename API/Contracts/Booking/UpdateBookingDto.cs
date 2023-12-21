using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.Booking;

public class UpdateBookingDto
{
    
    [Required(ErrorMessage = "Id is required")]
    [GuidValidation]
    public string Id { get; set; }
    
    [Required(ErrorMessage = "Status is required")]
    [Range(1, 2, ErrorMessage = "Status must be 1 or 2")]
    public int Status { get; set; }
}