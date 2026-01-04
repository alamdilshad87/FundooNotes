using System.Security.Cryptography;
using System.Text;

namespace ModelLayer.Helpers
{
    public class PasswordHasher
    {
        public static void CreateHash(string password, out string passwordHash, out string passwordSalt)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            passwordSalt = Convert.ToBase64String(saltBytes);

            string combined = password + passwordSalt;
            byte[] combinedBytes = Encoding.UTF8.GetBytes(combined);

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(combinedBytes);

            passwordHash = Convert.ToBase64String(hashBytes);
        }
        
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string combined = password + storedSalt;
            byte[] combinedBytes = Encoding.UTF8.GetBytes(combined);

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(combinedBytes);
            
            string computedHash = Convert.ToBase64String(hashBytes);
            
            return computedHash == storedHash;
        }
    }
}
