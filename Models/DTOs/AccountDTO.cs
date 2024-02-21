namespace HomeBankingMindHub.Models.DTOs
{
    public class AccountDTO
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public DateTime CreationDate { get; set; }

        public double Balance { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }
        public AccountDTO(long Id,string number, double Balance,ICollection<TransactionDTO> transactions,DateTime CreationDate) 
        {
            this.Id = Id;
            this.Number = number;
            this.CreationDate = CreationDate;
            this.Balance = Balance;
            Transactions = transactions;
        }
        public AccountDTO(long Id, string number, double Balance,DateTime CreationDate)
        {
            this.Id = Id;
            this.Number = number;
            this.CreationDate = CreationDate;
            this.Balance = Balance;
        }
    }
}
