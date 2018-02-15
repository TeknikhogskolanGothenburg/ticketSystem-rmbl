using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketShop.Models
{
    public class ApiInformation
    {
        public string Key { get; private set; }
        public string Secret { get; private set; }

        /// <summary>
        /// Default constructor, set api key and secret
        /// </summary>
        public ApiInformation()
        {
            Key = "";
            Secret = new Guid().ToString();
        }
    }
}
