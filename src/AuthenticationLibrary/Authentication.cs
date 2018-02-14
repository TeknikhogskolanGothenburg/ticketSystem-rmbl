using System;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationLibrary
{
    public static class Authentication
    {

        /// <summary>
        /// Prepare and return Authentication header's value
        /// </summary>
        /// <param name="key">Api key</param>
        /// <param name="secret">Api secret</param>
        /// <param name="timestamp">Timestamp to hash</param>
        /// <returns>Prepare value for Authentication header</returns>
        public static string AuthenticationHeader(string key, string secret, string timestamp)
        {
            string hashedTimestamp = HashMessageByKey(secret, timestamp);
            return key + ":" + hashedTimestamp;
        }

        /// <summary>
        /// Prepare and return User Authentication header's value
        /// </summary>
        /// <param name="id">Session index</param>
        /// <param name="secret">Session secret</param>
        /// <param name="timestamp">Timestamp to hash</param>
        /// <returns>Prepare value for User Authentication header</returns>
        public static string UserAuthenticationHeader(int id, string secret, string timestamp)
        {
            string hashSessionSecretTimestamp = HashMessageByKey(secret, timestamp);
            return id.ToString() + ":" + hashSessionSecretTimestamp;
        }

        /// <summary>
        /// Hash string with hash key string
        /// </summary>
        /// <param name="hashKey">Hash key string</param>
        /// <param name="message">What to hash</param>
        /// <returns>Hashed string</returns>
        public static string HashMessageByKey(string hashKey, string message)
        {
            byte[] key = Encoding.UTF8.GetBytes(hashKey.ToUpper());
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }

        /// <summary>
        /// Verify hash message from key is the same as new hash of same message 
        /// </summary>
        /// <param name="hashKey">Hash key string</param>
        /// <param name="message">What was hashed</param>
        /// <param name="hashMessage">Hashed string</param>
        /// <returns>Validation result</returns>
        public static bool VerifyHashMessageByKey(string hashKey, string message, string hashMessage)
        {
            string compareHash = HashMessageByKey(hashKey, message);

            return (compareHash == hashMessage);
        }
    }
}
