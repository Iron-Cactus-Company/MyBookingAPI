namespace Domain;

public class ExceptionHours
{
    public Guid Id { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    
    public int Type { get; set; }
    public string? Description { get; set; }
    
    public Guid OpeningHoursId { get; set; }
    public OpeningHours OpeningHours { get; set; }
}