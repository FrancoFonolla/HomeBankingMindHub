using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Models.DTOs
{
    public class ClientCreateDTO
    {
        [JsonIgnore]

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
