using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyGenerator
{
    public class KeyService
    {
        public static GenerateKeyResult GenerateKey(string userName, DateTime expirationDate)
        {
            string baseString = $"{userName}-ExpirationDate:{expirationDate:yyyyMMdd}-YourSecretSalt";

            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(baseString));
                string subscriptionKey = BitConverter.ToString(hashedBytes).Replace("-", "").Substring(0, 16); // Adjust length as needed

                return new GenerateKeyResult
                {
                    SubscriptionKeyWithExpiration = EncodeExpirationDate(subscriptionKey, expirationDate),
                    ExpirationDate = expirationDate
                };
            }
        }

         static string EncodeExpirationDate(string subscriptionKey, DateTime expirationDate)
        {
            string encodedExpiration = Convert.ToBase64String(Encoding.UTF8.GetBytes(expirationDate.ToString("yyyyMMdd")));
            return $"{subscriptionKey}-{encodedExpiration}";
        }

        public static DateTime DecodeExpirationDate(string subscriptionKeyWithExpiration)
        {
            string[] parts = subscriptionKeyWithExpiration.Split('-');
            if (parts.Length == 2)
            {
                string encodedExpiration = parts.LastOrDefault();
                byte[] decodedBytes = Convert.FromBase64String(encodedExpiration);
                string expirationString = Encoding.UTF8.GetString(decodedBytes);
                if (DateTime.TryParseExact(expirationString, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime expirationDate))
                {
                    return expirationDate;
                }
            }

            // Default value if decoding fails
            return DateTime.MinValue;
        }
    }

    public class GenerateKeyResult
    {
        public string SubscriptionKeyWithExpiration { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class RefCheck
    {
        public List<string> Error { get; set; }
    }
}

