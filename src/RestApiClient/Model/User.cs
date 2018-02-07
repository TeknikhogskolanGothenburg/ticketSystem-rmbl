using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { private get; set; }
        public Int16 Grade { get; set; }
    }
}
