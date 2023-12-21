using API.Enum;
using Application.DTOs;
using Domain;

namespace API.Service;

public class NotificationService
{
    public string GetNotificationContent(NotificationType type, BookingDto booking, Domain.Service service, Company company)
    {
        var content = "";
        switch (type)
        {
            case NotificationType.BookingMade:
                content = $"A booking has been made in your {company.Name} company, please go and accept/decline it in your profile.\n" +
                          $"Here is some additional information:\n" +
                          $"- Service name: {service.Name}\n" +
                          $"- Time: {GetDateString(booking.Start)} - {GetDateString(booking.End)}";
                break;
            case NotificationType.BookingAccepted:
                content = $"Your booking has been accepted.\n" +
                          $"Here is some additional information:\n" +
                          $"- Place: {company.Address}, {company.Name}\n" +
                          $"- Service name: {service.Name}\n" +
                          $"- Time: {GetDateString(booking.Start)} - {GetDateString(booking.End)}\n" +
                          $"- Price: {service.Price}";
                break;
            case NotificationType.BookingDeclined:
                content = $"Sorry, but your booking has been declined by {company.Name}.\n" +
                          $"Here is some additional information:\n" +
                          $"- Place: {company.Address}, {company.Name}\n" +
                          $"- Service name: {service.Name}\n" +
                          $"- Time: {GetDateString(booking.Start)} - {GetDateString(booking.End)}";
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

    private string GetDateString(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)timestamp);
        var date = dateTimeOffset.UtcDateTime;

        var hourStr = date.Hour < 10 ? $"0{date.Hour}" : date.Hour.ToString();
        var minStr = date.Minute < 10 ? $"0{date.Minute}" : date.Minute.ToString();
        
        return $"{hourStr}:{minStr} {date.Day}.{date.Month}.{date.Year}";
    }
}