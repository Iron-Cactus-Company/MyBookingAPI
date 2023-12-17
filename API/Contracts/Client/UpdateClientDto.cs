namespace API.Contracts.Client
{
    using API.Contracts.Shared;
    using System.ComponentModel.DataAnnotations;

    public class UpdateClientDto
    {
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
        
        //[CustomPhoneValidation(SupportedRegion.Finland , ErrorMessage = "Invalid Finnish number")]
        public string Phone { get; set; }
    }
}