using System.ComponentModel.DataAnnotations.Schema;

namespace ArangkadaAPI.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        public int Id { get; set; }
        public string? OperatorName { get; set; }
        public string? DriverName { get; set; }
        public float Amount { get; set; }
        public string? Date { get; set; }
    }
}