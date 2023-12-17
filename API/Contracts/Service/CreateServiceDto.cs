using API.Contracts.Shared;

namespace API.Contracts.Service
{
    using System.ComponentModel.DataAnnotations;

    public class CreateServiceDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Name { get; set; }
        
        [StringLength(300, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 86400, ErrorMessage = "Duration must be between 1 and 86400")]
        public int Duration { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Price must be a positive float number")]
        public float Price { get; set; }
        
        [Required(ErrorMessage = "CompanyId is required")]
        [GuidValidation]
        public Guid CompanyId { get; set; }
    }
}