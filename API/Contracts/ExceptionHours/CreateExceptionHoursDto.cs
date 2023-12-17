using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.ExceptionHours
{
    public class CreateExceptionHoursDto
    {
        [Required(ErrorMessage = "Start is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Start Time must be greater than 0")]
        public long Start { get; set; }

        [Required(ErrorMessage = "End is required")]
        [Range(1, long.MaxValue, ErrorMessage = "End Time must be greater than 0")]
        public long End { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Type is required")]
        [Range(0, 1, ErrorMessage = "Status must be between 0 and 1")]
        public int Type { get; set; }
       
        [Required(ErrorMessage = "OpeningHoursId is required")]
        [GuidValidation]
        public string OpeningHoursId { get; set; }
    }
}
