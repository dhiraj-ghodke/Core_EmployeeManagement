using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace EM.Repositories
{
    public class PasswordHasher
    {
        // Generate salt + hash and return in format: salt.hash
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // 16 bytes

            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8 // 32 bytes
            );

            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hashBytes);

            return $"{saltBase64}.{hashBase64}";
        }

        // Verify a password against a stored salt.hash string
        public static bool VerifyPassword(string password, string storedSaltDotHash)
        {
            var parts = storedSaltDotHash.Split('.');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            byte[] testHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );

            return CryptographicOperations.FixedTimeEquals(testHash, storedHash);
        }
    }
}