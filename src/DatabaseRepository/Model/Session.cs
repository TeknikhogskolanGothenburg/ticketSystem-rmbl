using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class Session
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Secret { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
    }
}
