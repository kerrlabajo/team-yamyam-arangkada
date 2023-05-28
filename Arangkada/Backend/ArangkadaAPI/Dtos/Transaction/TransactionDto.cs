namespace ArangkadaAPI.Dtos.Transaction
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string? OperatorName { get; set; }
        public string? DriverName { get; set; }
        public float Amount { get; set; }
        public string? Date { get; set; }
    }
}
