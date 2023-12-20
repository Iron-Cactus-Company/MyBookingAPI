using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.Booking;

public class UpdateBookingDto
{
    
    [Required(ErrorMessage = "Id is required")]
    [GuidValidation]
    public string Id { get; set; }
    
    [Range(1, long.MaxValue, ErrorMessage = "Start must be greater than 0")]
    public long Start { get; set; }
    
    [Range(1, long.MaxValue, ErrorMessage = "End must be greater than 0")]
    public long End { get; set; }
    
    [Range(0, 2, ErrorMessage = "Status must be between 0 and 2")]
    public int Status { get; set; }
    
    [GuidValidation]
    public string ServiceId { get; set; }

    [GuidValidation]
    public string ClientId { get; set; }
}