using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;
using API.Enum;

namespace API.Contracts.Notification;

public class SendNotificationDto
{
    [Range(0, 2, ErrorMessage = "Type must be between 0 and 2")]
    [Required(ErrorMessage = "Type is required")]
    public NotificationType Type { get; set; }
    
    [Required(ErrorMessage = "BookingId is required")]
    [GuidValidation]
    public string BookingId { get; set; }
}