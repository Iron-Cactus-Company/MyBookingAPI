using System.ComponentModel.DataAnnotations;
using API.Enum;

namespace API.Contracts.Notification;

public class SendNotificationDto
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "ToEmail is required")]
    public string ToEmail { get; set; }
    
    [Range(0, 2, ErrorMessage = "Type must be between 0 and 2")]
    [Required(ErrorMessage = "Type is required")]
    public NotificationType Type { get; set; }
}