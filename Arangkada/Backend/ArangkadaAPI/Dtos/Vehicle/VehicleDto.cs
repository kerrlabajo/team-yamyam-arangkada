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
        public string? DistinctionLabel { get; set; }
        public double RentFee { get; set; }
        private bool? _rentStatus;
        public bool RentStatus
        {
            get
            {
                return _rentStatus ?? false;
            }
            set
            {
                _rentStatus = value;
            }
        }
    }
}
