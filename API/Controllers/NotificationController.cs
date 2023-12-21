using API.Contracts.Notification;
using API.Service;
using API.Service.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class NotificationController : BaseApiController
{
    private readonly IEmailSender _emailSender;
    private readonly NotificationService _notificationService;

    public NotificationController(IEmailSender emailSender, NotificationService notificationService)
    {
        _emailSender = emailSender;
        _notificationService = notificationService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult SendNotification([FromBody] SendNotificationDto body)
    {
        var topic = _notificationService.GetNotificationTopic(body.Type);
        var content = _notificationService.GetNotificationContent(body.Type);
        
        var message = new Message(new string[] { body.ToEmail }, body.ToEmail, topic, content);
        //_emailSender.SendEmailAsync(message);
        
        return NoContent();
    }
}