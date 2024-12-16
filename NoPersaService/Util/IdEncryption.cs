using System.Security.Cryptography;
using System.Text;

namespace NoPersaService.Util
{
    public static class IdEncryption
    {
        public static string EncryptId(long id)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? throw new InvalidOperationException("Encryption key is not set."));
            aes.IV = new byte[16];

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var writer = new StreamWriter(cs);
            writer.Write(id);
            writer.Flush();
            cs.FlushFinalBlock();

            var encryptedData = ms.ToArray();
            var encryptedBase64 = Convert.ToBase64String(encryptedData);

            var hmac = ComputeHMAC(encryptedBase64);

            return $"{encryptedBase64}:{hmac}";
        }

        public static long? DecryptId(string? encryptedPayload, bool canBeNull = false)
        {

            if (encryptedPayload == null)
            {
                if (canBeNull)
                {
                    return null;
                }

                throw new ArgumentNullException("Id can not be null");
            }

            if ("0".Equals(encryptedPayload.Trim()))
            {
                return 0;
            }

            var parts = encryptedPayload.Trim().Split(':');
            if (parts.Length != 2)
            {
                throw new InvalidOperationException("Invalid Id Format.");
            }

            var encryptedBase64 = parts[0];
            var providedHmac = parts[1];

            var expectedHmac = ComputeHMAC(encryptedBase64);
            if (providedHmac != expectedHmac)
            {
                throw new InvalidOperationException("Invalid Signature.");
            }

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? throw new InvalidOperationException("Encryption key is not set."));
            aes.IV = new byte[16];

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(encryptedBase64));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return long.Parse(reader.ReadToEnd());
        }

        private static string ComputeHMAC(string data)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("HMAC_KEY") ?? throw new InvalidOperationException("HMAC key is not set.")));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hash);
        }
    }
}
