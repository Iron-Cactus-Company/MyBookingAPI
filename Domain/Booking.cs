namespace Domain;

public class Booking : IEntityWithId
{
    public Guid Id { get; set; }
    public long Time { get; set; }
    public int Status { get; set; }
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
}