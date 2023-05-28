using System.ComponentModel.DataAnnotations;

namespace ArangkadaAPI.Dtos.Operator
{
    public class OperatorCreationDto
    {
        [Required(ErrorMessage = "The full name is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the full name is 100 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "The username is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the username is 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the password is 50 characters.")]   
        public string? Password { get; set; }

        [Required(ErrorMessage = "The email address is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the email address is 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
       
    }
}
