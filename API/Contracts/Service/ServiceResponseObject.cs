namespace API.Contracts.Service;
using Domain;

public class ServiceResponseObject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public int Price { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
}