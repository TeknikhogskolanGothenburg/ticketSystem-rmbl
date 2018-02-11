using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class Flight
    {
        public DateTime DepartureDate { get; set; }
        public int DeparturePort { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int ArrivalPort { get; set; }
        public int Seats { get; set; }
    }
}
