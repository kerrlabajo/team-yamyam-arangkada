using System.ComponentModel.DataAnnotations;

namespace ArangkadaAPI.Dtos.Transaction
{
    public class TransactionCreationDto
    {
        [Required(ErrorMessage = "The Operator Name is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Operator Name is 100 characters.")]
        public string? OperatorName { get; set; }

        [Required(ErrorMessage = "The Driver Name is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Driver Name is 100 characters.")]
        public string? DriverName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount must be a positive number with up to 2 decimal places.")]
        [Range(0.00, float.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "The Date is required.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Date is 20 characters.")]
        public string? Date { get; set; }
    }
}
