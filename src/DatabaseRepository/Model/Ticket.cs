using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class Ticket
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int FlightID { get; set; }
        public int SeatNumber { get; set; }
        public int BookAt { get; set; }
    }
}