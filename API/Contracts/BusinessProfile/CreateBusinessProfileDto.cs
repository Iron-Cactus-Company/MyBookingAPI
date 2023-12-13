namespace API.Contracts.BusinessProfile
{
    using System.ComponentModel.DataAnnotations;

    public class CreateBusinessProfileDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Password { get; set; }
    }
}