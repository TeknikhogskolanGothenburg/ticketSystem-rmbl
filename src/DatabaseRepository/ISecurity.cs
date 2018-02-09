using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository
{
    public interface ISecurity
    {
        /// <summary>
        /// Generates a new random salt string
        /// </summary>        
        /// <returns>salt string</returns>
        //string GenerateSalt();

        /// <summary>
        /// Generates a new SAH256 encrypted password
        /// </summary>
        /// <param name="password">password to be encrypted</param>
        /// <param name="salt">salt to be used in encryption</param>
        /// <returns>SHA256hashed password string</returns>
        //string GenerateSHA256Hash(string password, string salt);

        /// <summary>
        /// checks credetials of user
        /// </summary>
        /// <param name="credentials">variable to be evaluated</param>
        /// <returns>bool indicating validity of credentials </returns>
        bool IsAuthorised(string credentials);
        
    }
}
