using System.Diagnostics.Eventing.Reader;

namespace HomeBankingMindHub.Models.DTOs
{
    public class LoanDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }
        public LoanDTO(long Id,string Name,double MaxAmount,string Payments) {
            this.Id = Id;
            this.Name = Name;
            this.MaxAmount = MaxAmount;
            this.Payments = Payments;

        }

    }
}
