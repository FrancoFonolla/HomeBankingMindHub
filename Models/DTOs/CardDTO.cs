namespace HomeBankingMindHub.Models.DTOs
{
    public class CardDTO
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ThruDate { get; set; }
        public CardDTO(long id, string cardHolder, string type, string color, string number, int cvv, DateTime? fromDate, DateTime? thruDate)
        {
            Id = id;
            CardHolder = cardHolder;
            Type = type;
            Color = color;
            Number = number;
            Cvv = cvv;
            FromDate = fromDate;
            this.ThruDate= thruDate;
        }
    }
}
