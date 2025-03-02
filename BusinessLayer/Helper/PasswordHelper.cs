
using System.Security.Cryptography;

namespace BusinessLayer.Helper
{
	internal class PasswordHelper
	{

		private const int SaltSize = 16; // 128 bits is a good size

		public static byte[] GenerateSalt()
		{
			using (var rng = RandomNumberGenerator.Create())
			{
				byte[] salt = new byte[SaltSize];
				rng.GetBytes(salt);
				return salt;
			}
		}


		private const int KeySize = 32; // 256 bits
		private const int Iterations = 10000; // Adjust this value

		public static byte[] HashPassword(string password, byte[] salt)
		{
			using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
			{
				return pbkdf2.GetBytes(KeySize);
			}
		}


		public static string HashAndStore(string password)
		{
			byte[] salt = GenerateSalt();
			byte[] hash = HashPassword(password, salt);
			string saltString = Convert.ToBase64String(salt);
			string hashString = Convert.ToBase64String(hash);
			return $"{saltString}:{hashString}";
		}

		public static bool VerifyPassword(string storedHash, string providedPassword)
		{
			string[] parts = storedHash.Split(':');
			byte[] salt = Convert.FromBase64String(parts[0]);
			byte[] storedPasswordHash = Convert.FromBase64String(parts[1]);
			byte[] providedPasswordHash = HashPassword(providedPassword, salt);
			return CryptographicOperations.FixedTimeEquals(storedPasswordHash, providedPasswordHash);
		}
	}
}
