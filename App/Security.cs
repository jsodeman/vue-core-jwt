using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace VueCoreJwt.App
{
	public static class Security
	{
		public static string HashPassword(string requestPassword, byte[] salt)
		{
			var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				requestPassword,
				salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));
			return hash;
		}

		public static byte[] GenerateSalt()
		{
			var salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			return salt;
		}

		// generates a GUID for password resetting
		public static string GeneratePasswordResetIdentifier()
		{
			return Guid.NewGuid().ToString("N");
		}

		public static string GenerateRandomPassword()
		{
			return GetRandomString(8);
		}

		private static string GetRandomString(int length)
		{
			var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
			var data = new byte[1];
			using (var crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetNonZeroBytes(data);
				data = new byte[length];
				crypto.GetNonZeroBytes(data);
			}

			var result = new StringBuilder(length);
			foreach (var b in data)
			{
				result.Append(chars[b % (chars.Length)]);
			}

			return result.ToString();
		}

	}
}
