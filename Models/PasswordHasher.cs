using System.Security.Cryptography;

namespace HomeBankingMindHub.Models
{
    public class PasswordHasher
    {
        // Número de iteraciones utilizado para fortalecer la función hash
        private const int Iterations = 10000;
        // Longitud de la clave derivada
        private const int KeyLength = 32;

        // Método para generar un hash seguro de la contraseña
        public static (byte[],byte[]) HashPassword(string password)
        {
            // Generar un salt aleatorio
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Crear la instancia de la clase Rfc2898DeriveBytes con la contraseña y el salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);

            // Obtener el hash de la contraseña
            byte[] hash = pbkdf2.GetBytes(KeyLength);
            return (salt,hash);
        }

        // Método para verificar una contraseña contra su hash
        public static bool VerifyPassword(string enteredPassword, byte[] hash, byte[] salt)
        {
           

            

            // Crear una instancia de la clase Rfc2898DeriveBytes con el salt extraído
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations);

            // Obtener el hash de la contraseña ingresada
            byte[] hashentered = pbkdf2.GetBytes(KeyLength);

            // Comparar los hashes
            for (int i = 0; i < KeyLength; i++)
            {
                if (hashentered[i] != hash[i])
                    return false;
            }

            return true;
        }
    }

    
}
