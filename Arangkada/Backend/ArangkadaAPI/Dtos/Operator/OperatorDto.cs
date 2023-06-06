namespace ArangkadaAPI.Dtos.Operator
{
    public class OperatorDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? VerificationStatus { get; set; }
        public int? Vehicles { get; set; }
        public int? Drivers { get; set; }
        public string? VerificationCode { get; set; }
    }
}
