namespace Domain;

public class Company : IEntityWithId
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public string AcceptingTimeText { get; set; }
    public string CancellingTimeText { get; set; }
    
    public Guid BusinessProfileId { get; set; }
    public BusinessProfile BusinessProfile { get; set; }
    
    public OpeningHours? OpeningHours { get; set; }

    public ICollection<Service> Services { get; set; } = new List<Service>();
}