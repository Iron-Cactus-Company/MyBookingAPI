namespace API.Contracts.Client
{
    using API.Contracts.Shared;
    using System.ComponentModel.DataAnnotations;

    public class CreateClientDto
    {
        [Required(ErrorMessage = "name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
        
        [CustomPhoneValidation(SupportedRegion.Finland , ErrorMessage = "Invalid Finnish number")]
        public string Phone { get; set; }
    }
}