namespace ArangkadaAPI.Dtos.Driver
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string? OperatorName { get; set; }
        public string? VehicleAssigned { get; set; } = null;
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public string? DLCodes { get; set; }
    }
}
