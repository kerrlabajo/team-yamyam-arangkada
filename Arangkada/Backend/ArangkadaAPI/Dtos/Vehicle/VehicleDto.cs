namespace ArangkadaAPI.Dtos.Vehicle
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string? OperatorName { get; set; }
        public string? CRNumber { get; set; }
        public string? PlateNumber { get; set; }
        public string? BodyType { get; set; }
        public string? Make { get; set; }
        public double RentFee { get; set; }
        public bool RentStatus { get; set; }
    }
}
