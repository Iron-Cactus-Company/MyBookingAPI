namespace Domain;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public float Price { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}