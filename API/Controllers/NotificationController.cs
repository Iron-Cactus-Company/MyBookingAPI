using API.Contracts.Notification;
using API.Enum;
using API.Service;
using API.Service.Email;
using Application.BookingActions;
using Application.Core.Error.Enums;
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
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto body)
    {
        var booking = await Mediator.Send(new GetOne.Query{ Id = new Guid(body.BookingId) });
        if (booking.Error.Type == ErrorType.NotFound)
            return NotFound(booking.Error.Field = "BookingId");
        var service = await Mediator.Send(new Application.ServiceActions.GetOne.Query{ Id = booking.Value.ServiceId });
        if (service.Error.Type == ErrorType.NotFound)
            return NotFound(service.Error.Field = "Booking.ServiceId");
        
        var company = await Mediator.Send(new Application.CompanyActions.GetOne.Query{ Id = service.Value.CompanyId });
        if (company.Error.Type == ErrorType.NotFound)
            return NotFound(service.Error.Field = "Service.CompanyId");
        
        var topic = _notificationService.GetNotificationTopic(body.Type);
        var content = _notificationService.GetNotificationContent(body.Type, booking.Value, service.Value, company.Value);

        var receiverEmail = "";
        if (body.Type == NotificationType.BookingMade)
        {
            var businessProfile = await Mediator.Send(new Application.BusinessProfileActions.GetOne.Query{ Id = company.Value.BusinessProfileId });
            if (businessProfile.Error.Type == ErrorType.NotFound)
                return NotFound(booking.Error.Field = "Company.BusinessProfileId");

            receiverEmail = businessProfile.Value.Email;
        }
        else
        {
            var client = await Mediator.Send(new Application.ClientActions.GetOne.Query{ Id = booking.Value.ClientId });
            if (client.Error.Type == ErrorType.NotFound)
                return NotFound(booking.Error.Field = "Booking.ClientId");

            receiverEmail = client.Value.Email;
        }
        
        var message = new Message(new string[] { receiverEmail }, receiverEmail, topic, content);
        //_emailSender.SendEmailAsync(message);
        
        return NoContent();
    }
}