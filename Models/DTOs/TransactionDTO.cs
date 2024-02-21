namespace HomeBankingMindHub.Models.DTOs
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TransactionDTO(long id, string type, double amount, string description, DateTime date)
        {
            Id = id;
            Type = type;
            Amount = amount;
            Description = description;
            Date = date;
        }
    }
}
