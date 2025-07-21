using System.Security.Cryptography;

namespace Service.Lib.SecureCodeGenerator
{
    public static class SecureCodeGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateSecureInviteCode(int length = 8)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than 0.");

            var code = new char[length];
            byte[] randomBytes = new byte[length];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            for (int i = 0; i < length; i++)
            {
                int index = randomBytes[i] % Chars.Length;
                code[i] = Chars[index];
            }

            return new string(code);
        }
    }
}