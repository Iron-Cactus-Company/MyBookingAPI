using API.Contracts.Shared;

namespace API.Contracts.Service
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateServiceDto
    {
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }
        
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Name { get; set; }
        
        [StringLength(300, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Description { get; set; }
        
        [Range(1, 86400, ErrorMessage = "Duration must be between 0 and 24")]
        public int Duration { get; set; }
        
        [Range(0, float.MaxValue, ErrorMessage = "Price must be a positive float number")]
        public float Price { get; set; }
        
        [GuidValidation]
        public string CompanyId { get; set; }
    }
}