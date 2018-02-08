using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Epost { get; set; }
        public Int16 Grade { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}
