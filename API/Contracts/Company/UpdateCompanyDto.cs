namespace API.Contracts.Company
{
    using Shared;
    using System.ComponentModel.DataAnnotations;

    public class UpdateCompanyDto
    {
        [Required(ErrorMessage = "Id is required")]
        [GuidValidation]
        public string Id { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Name { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Address { get; set; }

        [CustomPhoneValidation(SupportedRegion.Finland , ErrorMessage = "Invalid Finnish number")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string Description { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string AcceptingTimeText { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} character(s) in length.")]
        public string CancellingTimeMessage { get; set; }
        
        [GuidValidation(ErrorMessage = "BusinessProfileId must be a Guid")]
        public string BusinessProfileId { get; set; }
    }
}