using System.ComponentModel.DataAnnotations.Schema;

namespace ArangkadaAPI.Models
{
    [Table("Operator")]
    public class Operator
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool? IsVerified { get; set; }
        public int? Vehicles { get; set; }
        public int? Drivers { get; set; }
        public string? VerificationCode { get; set; }
    }
}
