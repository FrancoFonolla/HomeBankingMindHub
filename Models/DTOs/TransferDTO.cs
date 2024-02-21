namespace HomeBankingMindHub.Models.DTOs
{
    public class TransferDTO
    {
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set;   }
        public double Amount { get; set; }
        public string Description { get; set; }
        public TransferDTO(string FromAccountNumber,string ToAccountNumber,double Amount,string Description)
        {
            this.FromAccountNumber = FromAccountNumber;
            this.ToAccountNumber = ToAccountNumber;
            this.Amount = Amount;
            this.Description = Description;

        }

    }
}
