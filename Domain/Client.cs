namespace Domain;

public class Client : IEntityWithId
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}