namespace Domain;

public class BusinessProfile : IEntityWithId
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Company> Companies { get; set; } = new List<Company>();
}