using System.Text;
using System.Text.RegularExpressions;

namespace HomeBankingMindHub.Models

{
    public class Utiles
    {
        public static string GeneratedRandomVIN()
        {
            Random random = new Random();
            string randomNumber = random.Next(0, 100000000).ToString("D8");

            return ("VIN-" + randomNumber);
        }
        public static string GenerateRandomCardNumber()
        {
            var random = new Random();
            var number = string.Join("-", Enumerable.Range(0, 4)
                .Select(_ => random.Next(10000))
                .Select(n => n.ToString("0000")));
            Console.WriteLine(number);
            return number;
        }
        public static int GenerateRandomCardCvv() {
            var random = new Random();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:D3}",random.Next(0,1000));
            return Int32.Parse(sb.ToString().Trim());
        }
        public static bool IsValidEmail(string email)
        {
            // Expresión regular para verificar el formato de la dirección de correo electrónico
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Verifica si la cadena coincide con el patrón
            return Regex.IsMatch(email, pattern);
        }
    }
    
}
