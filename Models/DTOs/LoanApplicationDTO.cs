namespace HomeBankingMindHub.Models.DTOs
{
    public class LoanApplicationDTO
    {
        public long LoanId { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public string ToAccountNumber { get; set; }
        public LoanApplicationDTO(long loanId, double amount, string payments, string toAccountNumber)
        {
            LoanId = loanId;
            Amount = amount;
            Payments = payments;
            ToAccountNumber = toAccountNumber;
        }
    }
}
