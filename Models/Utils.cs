namespace HomeBankingMindHub.Models
{
    public class Utils
    {
        public string GeneratedRandomVIN()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            return "VIN-" + randomNumber;
        }
    }
}
