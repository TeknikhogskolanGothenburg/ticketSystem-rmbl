using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class SessionInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int UserGrade { get; set; }
        public int SessionId { get; set; }
        public string SessionSecret { get; set; }
    }
}
