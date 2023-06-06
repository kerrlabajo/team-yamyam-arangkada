using System.ComponentModel.DataAnnotations;

namespace ArangkadaAPI.Dtos.Vehicle
{
    public class VehicleCreationDto
    {
        [Required(ErrorMessage = "The operator name is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the operator name is 100 characters.")]
        
        public string? OperatorName { get; set; }

        [Required(ErrorMessage = "The CR number is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the CR number is 15 characters.")]
        public string? CRNumber { get; set; }

        [Required(ErrorMessage = "Plate number is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the plate number is 10 characters.")]
        public string? PlateNumber { get; set; }

        [Required(ErrorMessage = "Body type is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the body type is 50 characters.")]
        public string? BodyType { get; set; }

        [Required(ErrorMessage = "Make is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the make is 10 characters.")]
        public string? Make { get; set; }

        [Required(ErrorMessage = "Distinction label is required.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the distinction label is 10 characters.")]
        public string? DistinctionLabel { get; set; }

        [Required(ErrorMessage = "Rent fee is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Rent fee must be at least 1")]
        public double RentFee { get; set; }
    }
}
