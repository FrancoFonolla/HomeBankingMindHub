namespace HomeBankingMindHub.Models

{
    public class Utiles
    {
        public  string GeneratedRandomVIN()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100000000);
            return ("VIN-" + randomNumber);
        }
    }
}
