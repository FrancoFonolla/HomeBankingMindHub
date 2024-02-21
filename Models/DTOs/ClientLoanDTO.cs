namespace HomeBankingMindHub.Models.DTOs
{
    public class ClientLoanDTO
    {
        public long Id {  get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
        public ClientLoanDTO(long id, long loanId, string name, double amount, int payments)
        {
            Id = id;
            LoanId = loanId;
            Name = name;
            Amount = amount;
            Payments = payments;
        }
    }
}
