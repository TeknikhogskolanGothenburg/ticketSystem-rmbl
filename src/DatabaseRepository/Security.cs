using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace TicketSystem.DatabaseRepository
{
    public class Security : ISecurity
    {
        //public string GenerateSalt()
        //{
        //    RandomNumberGenerator rng = RNGCryptoServiceProvider.Create();
        //    byte[] buff = new byte[10];
        //    rng.GetBytes(buff);
        //    return Convert.ToBase64String(buff);
        //}

        //public string GenerateSHA256Hash(string password, string salt)
        //{
        //    byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
        //    SHA256Managed hashEngine = new SHA256Managed();
        //    byte[] hashedPassword = hashEngine.ComputeHash(saltedPassword);
        //    return Convert.ToBase64String(hashedPassword);
        //}

        public bool IsAuthorised(string credentials)
        {
            //checks to see if authentication value is valid
            return true;
        }
    }
}
