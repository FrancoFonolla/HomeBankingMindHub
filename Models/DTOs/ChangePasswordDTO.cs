namespace HomeBankingMindHub.Models.DTOs
{
    public class ChangePasswordDTO
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public ChangePasswordDTO(string Email,string CurrentPassword,string NewPassword) 
        {
            this.Email = Email;
            this.CurrentPassword = CurrentPassword;
            this.NewPassword = NewPassword;
        }
    }
}
