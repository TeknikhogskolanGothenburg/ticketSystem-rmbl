using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class LoginAnswer
    {
        public int SessionId { get; set; }
        public string SessionSecret { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int UserGrade { get; set; }
    }
}
