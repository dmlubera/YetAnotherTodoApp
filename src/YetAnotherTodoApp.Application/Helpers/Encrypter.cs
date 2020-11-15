using System;
using System.Security.Cryptography;
using System.Text;

namespace YetAnotherTodoApp.Application.Helpers
{
    public class Encrypter : IEncrypter
    {
        private static int _saltBytesSize = 40;
        private static int _deriveBytesIterationsCount = 10000;

        public string GetSalt()
        {
            var salt = new byte[_saltBytesSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public string GetHash(string value, string salt)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Cannot generate hash from an empty value.");
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("Cannot use empty an empty salt for hashing value.");

            var rfc2898 = new Rfc2898DeriveBytes(value, Encoding.ASCII.GetBytes(salt),
            _deriveBytesIterationsCount, HashAlgorithmName.SHA256);

            return Convert.ToBase64String(rfc2898.GetBytes(_saltBytesSize));
        }
    }
}