namespace Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }
    public long Start { get; set; }
    public long End { get; set; }
    public int Status { get; set; }
    
    public Guid ServiceId { get; set; }
    
    public Guid ClientId { get; set; }
    public ClientDto Client { get; set; }
}