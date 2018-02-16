using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using AuthenticationLibrary;
using TicketSystem.DatabaseRepository;
using TicketSystem.DatabaseRepository.Model;

namespace TicketShopAPI.APISecurity
{
    public class Security : ISecurity
    {
        public string Timestamp;
        private TicketDatabase TicketDb = new TicketDatabase();
        public string ApiKey;
        private string ApiKeyHashTimestamp;
        public string ApiSecret;
        public int UserId;
        private string SessionHashTimestamp;
        private Session Session;
        public User User;
        

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

        public bool IsAuthorised(string timestamp, string apiKeyData, string sessionData, int grade)
        {
            if (apiKeyData == null)
            {
                return false;
            }

            Timestamp = timestamp;
            string[] keyParts = apiKeyData.Split(':');
            ApiKey = keyParts[0];
            ApiKeyHashTimestamp = keyParts[1];
            ApiSecret = TicketDb.APISecretFind(ApiKey);

            if (!Authentication.VerifyHashMessageByKey(ApiKey, Timestamp, ApiKeyHashTimestamp))
            {
                return false;
            }

            bool SessionVerified = false;

            if (!(sessionData == null))
            {
                InitilazeSessionData(sessionData);
                SessionVerified = VerifySession();

                if (SessionVerified)
                {
                    User = TicketDb.UserFind(Session.UserID);
                }
            }

            // grade -1: everyone are allowed to get/send data
            // grade < 1, no login visiter are allowed
            // If accepted grade are under and equal with session user grade, allow to get/send data
            if ((grade < 0) || (grade == 0 && !SessionVerified) || ((SessionVerified) && grade <= User.Grade))
            {
                return true;
            }

            return false;
        }

        private bool VerifySession()
        {
            if (Authentication.VerifyHashMessageByKey(Session.Secret, Timestamp, SessionHashTimestamp))
            {
                return true;
            }
            return false;
        }

        private void InitilazeSessionData(string Data)
        {
            string[] sessionParts = Data.Split(':');
            int sessionId;

            if (int.TryParse(sessionParts[0], out sessionId))
            {
                SessionHashTimestamp = sessionParts[1];
                Session = TicketDb.SessionFind(sessionId);
            }
        }
    }
}
