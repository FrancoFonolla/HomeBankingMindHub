namespace HomeBankingMindHub.Models.DTOs
{
    public class ClientLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ClientLoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
