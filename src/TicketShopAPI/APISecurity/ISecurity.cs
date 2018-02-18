using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShopAPI.APISecurity
{
    public interface ISecurity
    {
        /// <summary>
        /// checks credetials of user
        /// </summary>
        /// <param name="timestamp">date</param>
        /// <param name="apiKeyData">apikey check</param>
        /// <param name="sessionData">login check/param>
        /// <param name="grade"> -1 = no restriction, 0 = only non-users, 1 = customer, 2 = administrator</param>
        /// <returns>bool indicating validation success</returns>
        bool IsAuthorised(string timestamp, string apiKeyData, string sessionData, int grade);
        
    }
}
