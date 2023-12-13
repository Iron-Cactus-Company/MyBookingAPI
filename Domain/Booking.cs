namespace Domain;

public class Booking
{
    public Guid Id { get; set; }
    public int Time { get; set; }
    public int Status { get; set; }
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
}