namespace API.Contracts.OpeningHours;

public class OpeningHoursResponseObject
{
    public Guid Id { get; set; }
    public long MondayStart { get; set; }
    public long MondayEnd { get; set; }
    
    public long TuesdayStart { get; set; }
    public long TuesdayEnd { get; set; }
    
    public long WednesdayStart { get; set; }
    public long WednesdayEnd { get; set; }
    
    public long ThursdayStart { get; set; }
    public long ThursdayEnd { get; set; }
    
    public long FridayStart { get; set; }
    public long FridayEnd { get; set; }
    
    public long SaturdayStart { get; set; }
    public long SaturdayEnd { get; set; }
    
    public long SundayStart { get; set; }
    public long SundayEnd { get; set; }
    
    public Guid CompanyId { get; set; }
}