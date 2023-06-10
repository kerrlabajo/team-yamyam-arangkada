namespace ArangkadaAPI.Dtos.Transaction
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string? OperatorName { get; set; }
        public string? DriverName { get; set; }
        private float? _amount { get; set; }
        public float Amount
        {
            get
            {
                return _amount ?? 0;
            }
            set
            {
                _amount = value;
            }
        }
        private string? _status { get; set; }
        public string? Status
        {
            get
            {
                return _status ?? "Pending";
            }
            set
            {
                _status = value;
            }
        }
        public string? StartDate { get; set; }

        private string? _endDate { get; set; }
        public string? EndDate
        {
            get
            {
                return _endDate ?? "N/A";
            }
            set
            {
                _endDate = value;
            }
        }
    }
}
