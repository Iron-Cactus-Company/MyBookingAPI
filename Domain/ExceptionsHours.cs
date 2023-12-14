namespace Domain;

public class ExceptionHours
{
    public Guid Id { get; set; }
    public long Start { get; set; }
    public long End { get; set; }
    
    public int Type { get; set; }
    public string? Description { get; set; }
    
    public Guid OpeningHoursId { get; set; }
    public OpeningHours OpeningHours { get; set; }
}