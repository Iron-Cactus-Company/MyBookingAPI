using API.Contracts.Shared;

namespace API.Contracts.BusinessProfile
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateBusinessProfileDto
    {
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }
        
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Password { get; set; }
    }
}