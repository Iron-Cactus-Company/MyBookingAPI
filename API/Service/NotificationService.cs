using API.Enum;

namespace API.Service;

public class NotificationService
{
    public string GetNotificationContent(NotificationType type)
    {
        var content = "";
        switch (type)
        {
            case NotificationType.BookingMade:
                content = "Somebody has made a booking, go check that";
                break;
            case NotificationType.BookingAccepted:
                content = "Your booking is accepted";
                break;
            case NotificationType.BookingDeclined:
                content = "Sorry, but your booking is not accepted";
                break;
        }

        content = WrapContent(content);
        return content;
    }
    
    public string GetNotificationTopic(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.BookingMade:
                return "New Booking";
            case NotificationType.BookingAccepted:
                return "Booking Accepted";
            case NotificationType.BookingDeclined:
                return "Booking Declined";
        }

        return "";
    }

    private string WrapContent(string content)
    {
        return $"<div style=\"font-size: 20px; font-family: 'Roboto', sans-serif;\">" +
               $"{content}" +
               $"</div>";
    }
}