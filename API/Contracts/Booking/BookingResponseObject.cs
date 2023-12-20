using Application.DTOs;

namespace API.Contracts.Booking;
using Domain;    

public class BookingResponseObject
{
    public Guid Id { get; set; }
    public long Start { get; set; }
    public long End { get; set; }
    public int Status { get; set; }
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    
    public Guid ClientId { get; set; }
    public ClientDto Client { get; set; }
}