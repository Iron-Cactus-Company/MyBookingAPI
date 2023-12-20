using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.OpeningHours
{
    public class UpdateOpeningHoursDto
    {
        
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }
        
        public string MondayStart { get; set; }
        
        public string MondayEnd { get; set; }
        
        public string TuesdayStart { get; set; }
        
        public string TuesdayEnd { get; set; }
        
        public string WednesdayStart { get; set; }
        
        public string WednesdayEnd { get; set; }
        
        public string ThursdayStart { get; set; }
        
        public string ThursdayEnd { get; set; }
        
        public string FridayStart { get; set; }
        
        public string FridayEnd { get; set; }
        
        public string SaturdayStart { get; set; }
        
        public string SaturdayEnd { get; set; }
        
        public string SundayStart { get; set; }
        
        public string SundayEnd { get; set; }
        
    }
}
