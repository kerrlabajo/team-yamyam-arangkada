namespace ArangkadaAPI.Dtos.Driver
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string? OperatorName { get; set; }
        private string? _vehicleAssigned;
        public string? VehicleAssigned
        {
            get
            {
                return _vehicleAssigned;
            }
            set
            {
                if (value == null)
                {
                    _vehicleAssigned = "Not Currently Renting";
                }
                else
                {
                    _vehicleAssigned = value;
                }
            }
        }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public string? DLCodes { get; set; }
        public string? Category { get; set; }
    }
}
