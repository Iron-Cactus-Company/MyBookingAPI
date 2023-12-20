using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.AvailableHours
{
    public class GenerateAvailableHoursDto
    {
        [Required(ErrorMessage = "ServiceId is required")]
        public string ServiceId{ get; init; }
        
        [Required(ErrorMessage = "From is required")]
        public long From{ get; init; }
        
        [Required(ErrorMessage = "To is required")]
        public long To{ get; init; }
    }
}
