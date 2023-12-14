using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.OpeningHours
{
    public class UpdateOpeningHoursDto
    {
        
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "MondayStart Time must be greater than 0")]
        public long MondayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "MondayEnd Time must be greater than 0")]
        public long MondayEnd { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "TuesdayStart Time must be greater than 0")]
        public long TuesdayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "TuesdayEnd Time must be greater than 0")]
        public long TuesdayEnd { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "WednesdayStart Time must be greater than 0")]
        public long WednesdayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "WednesdayEnd Time must be greater than 0")]
        public long WednesdayEnd { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "ThursdayStart Time must be greater than 0")]
        public long ThursdayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "ThursdayEnd Time must be greater than 0")]
        public long ThursdayEnd { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "FridayStart Time must be greater than 0")]
        public long FridayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "FridayEnd Time must be greater than 0")]
        public long FridayEnd { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "SaturdayStart Time must be greater than 0")]
        public long SaturdayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "SaturdayEnd Time must be greater than 0")]
        public long SaturdayEnd { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "SundayStart Time must be greater than 0")]
        public long SundayStart { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "SundayEnd Time must be greater than 0")]
        public long SundayEnd { get; set; }
        
    }
}
