namespace Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }
    public long Time { get; set; }
    public int Status { get; set; }
    
    public Guid ServiceId { get; set; }
    
    public ClientDto Client { get; set; }
}