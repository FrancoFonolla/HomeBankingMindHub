using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Models.DTOs
{
    public class ClientDTO
    {
        [JsonIgnore]

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<AccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Credits { get; set; }
        public ICollection<CardDTO> Cards { get; set; }
        public ClientDTO(long id, string firstName, string lastName, string email, ICollection<AccountDTO> accounts, ICollection<ClientLoanDTO> credits, ICollection<CardDTO> cards)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Accounts = accounts;
            Credits = credits;
            Cards = cards;
        }
    }
}
