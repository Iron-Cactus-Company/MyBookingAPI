﻿namespace API.Contracts.ExceptionHours;

public class ExceptionHoursResponseObject
{
    public long Start { get; set; }
    
    public long End { get; set; }
    
    public string Description { get; set; }
    
    public int Type { get; set; }
    
    public Guid OpeningHoursId { get; set; }
}