using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShopAPI.APISecurity
{
    public interface ISecurity
    {
        /// <summary>
        /// Generates a new random salt string
        /// DEACTIVATED
        /// </summary>        
        /// <returns>salt string</returns>
        //string GenerateSalt();

        /// <summary>
        /// Generates a new SAH256 encrypted password
        /// DEACTIVATED
        /// </summary>
        /// <param name="password">password to be encrypted</param>
        /// <param name="salt">salt to be used in encryption</param>
        /// <returns>SHA256hashed password string</returns>
        //string GenerateSHA256Hash(string password, string salt);

        /// <summary>
        /// checks credetials of user
        /// </summary>
        /// <param name="timestamp">date</param>
        /// <param name="apiKeyData">apikey check</param>
        /// <param name="sessionData">login check/param>
        /// <param name="grade">1 = customer, 2 = administrator</param>
        /// <returns>bool indicating validation success</returns>
        bool IsAuthorised(string timestamp, string apiKeyData, string sessionData, int grade);
        
    }
}
