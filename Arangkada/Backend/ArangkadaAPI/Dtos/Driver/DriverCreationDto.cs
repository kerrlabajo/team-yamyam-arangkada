using System.ComponentModel.DataAnnotations;

namespace ArangkadaAPI.Dtos.Driver
{
    public class DriverCreationDto
    {
        [MaxLength(100, ErrorMessage = "Maximum length for the Operator Name is 100 characters.")]
        public string? OperatorName { get; set; }

        [Required(ErrorMessage = "The full name is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the full name is 100 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "The Address is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Address is 150 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "The Contact Number is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Contact Number is 50 characters.")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "The License Number is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the License Number is 50 characters.")]
        public string? LicenseNumber { get; set; }

        [Required(ErrorMessage = "The Expiration Date is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Expiration Date is 50 characters.")]
        public string? ExpirationDate { get; set; }

        [Required(ErrorMessage = "The DLCodes is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the DLCodes is 20 characters.")]
        public string? DLCodes { get; set; }
    }
}
