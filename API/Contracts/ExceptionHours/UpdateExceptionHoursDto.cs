using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.ExceptionHours
{
    public class UpdateExceptionHoursDto
    {
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "Start Time must be greater than 0")]
        public long Start { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessage = "End Time must be greater than 0")]
        public long End { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Description { get; set; }
        
        [Range(0, 1, ErrorMessage = "Status must be between 0 and 1")]
        public int Type { get; set; }
        
    }
}