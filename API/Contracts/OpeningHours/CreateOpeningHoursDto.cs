using System.ComponentModel.DataAnnotations;
using API.Contracts.Shared;

namespace API.Contracts.OpeningHours
{
    public class CreateOpeningHoursDto
    {
        [Required(ErrorMessage = "MondayStart is required")]
        public string MondayStart { get; set; }

        [Required(ErrorMessage = "MondayEnd is required")]
        public string MondayEnd { get; set; }

        [Required(ErrorMessage = "TuesdayStart is required")]
        public string TuesdayStart { get; set; }

        [Required(ErrorMessage = "TuesdayEnd is required")]
        public string TuesdayEnd { get; set; }

        [Required(ErrorMessage = "WednesdayStart is required")]
        public string WednesdayStart { get; set; }

        [Required(ErrorMessage = "WednesdayEnd is required")]
        public string WednesdayEnd { get; set; }

        [Required(ErrorMessage = "ThursdayStart is required")]
        public string ThursdayStart { get; set; }

        [Required(ErrorMessage = "ThursdayEnd is required")]
        public string ThursdayEnd { get; set; }

        [Required(ErrorMessage = "FridayStart is required")]
        public string FridayStart { get; set; }

        [Required(ErrorMessage = "FridayEnd is required")]
        public string FridayEnd { get; set; }

        [Required(ErrorMessage = "SaturdayStart is required")]
        public string SaturdayStart { get; set; }

        [Required(ErrorMessage = "SaturdayEnd is required")]
        public string SaturdayEnd { get; set; }

        [Required(ErrorMessage = "SundayStart is required")]
        public string SundayStart { get; set; }

        [Required(ErrorMessage = "SundayEnd is required")]
        public string SundayEnd { get; set; }

        [Required(ErrorMessage = "CompanyId is required")]
        [GuidValidation]
        public string CompanyId { get; set; }
    }
}
