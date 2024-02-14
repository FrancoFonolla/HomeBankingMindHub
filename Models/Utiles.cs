using System.Text;

namespace HomeBankingMindHub.Models

{
    public class Utiles
    {
        public  string GeneratedRandomVIN()
        {
            Random random = new Random();
            string randomNumber = random.Next(0, 100000000).ToString("D8");

            return ("VIN-" + randomNumber);
        }
        public string GenerateRandomCardNumber()
        {
            var random = new Random();
            var number = string.Join("-", Enumerable.Range(0, 4)
                .Select(_ => random.Next(10000))
                .Select(n => n.ToString("0000")));
            Console.WriteLine(number);
            return number;
        }
        public int GenerateRandomCardCvv() {
            var random = new Random();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:D3}",random.Next(0,1000));
            return Int32.Parse(sb.ToString().Trim());
        }
    }
    
}
