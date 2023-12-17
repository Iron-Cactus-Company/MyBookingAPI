namespace API.Contracts.Company
{
    using Shared;
    using System.ComponentModel.DataAnnotations;

    public class CreateCompanyDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Name { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Address { get; set; }

        //[CustomPhoneValidation(SupportedRegion.Finland , ErrorMessage = "Invalid Finnish number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Description { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        [Required(ErrorMessage = "Accepting time text is required")]
        public string AcceptingTimeText { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        [Required(ErrorMessage = "Cancelling time message is required")]
        public string CancellingTimeMessage { get; set; }
        
        [Required(ErrorMessage = "BusinessProfileId is required")]
        [GuidValidation]
        public string BusinessProfileId { get; set; }
    }
}