using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.OpeningHours
{
    public class CreateOpeningHoursDto
    {
        [Required(ErrorMessage = "MondayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "MondayStart Time must be greater than 0")]
        public long MondayStart { get; set; }

        [Required(ErrorMessage = "MondayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "MondayEnd Time must be greater than 0")]
        public long MondayEnd { get; set; }

        [Required(ErrorMessage = "TuesdayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "TuesdayStart Time must be greater than 0")]
        public long TuesdayStart { get; set; }

        [Required(ErrorMessage = "TuesdayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "TuesdayEnd Time must be greater than 0")]
        public long TuesdayEnd { get; set; }

        [Required(ErrorMessage = "WednesdayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "WednesdayStart Time must be greater than 0")]
        public long WednesdayStart { get; set; }

        [Required(ErrorMessage = "WednesdayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "WednesdayEnd Time must be greater than 0")]
        public long WednesdayEnd { get; set; }

        [Required(ErrorMessage = "ThursdayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "ThursdayStart Time must be greater than 0")]
        public long ThursdayStart { get; set; }

        [Required(ErrorMessage = "ThursdayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "ThursdayEnd Time must be greater than 0")]
        public long ThursdayEnd { get; set; }

        [Required(ErrorMessage = "FridayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "FridayStart Time must be greater than 0")]
        public long FridayStart { get; set; }

        [Required(ErrorMessage = "FridayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "FridayEnd Time must be greater than 0")]
        public long FridayEnd { get; set; }

        [Required(ErrorMessage = "SaturdayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "SaturdayStart Time must be greater than 0")]
        public long SaturdayStart { get; set; }

        [Required(ErrorMessage = "SaturdayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "SaturdayEnd Time must be greater than 0")]
        public long SaturdayEnd { get; set; }

        [Required(ErrorMessage = "SundayStart is required")]
        [Range(1, long.MaxValue, ErrorMessage = "SundayStart Time must be greater than 0")]
        public long SundayStart { get; set; }

        [Required(ErrorMessage = "SundayEnd is required")]
        [Range(1, long.MaxValue, ErrorMessage = "SundayEnd Time must be greater than 0")]
        public long SundayEnd { get; set; }

        [Required(ErrorMessage = "CompanyId is required")]
        [GuidValidation]
        public Guid CompanyId { get; set; }
    }
}
