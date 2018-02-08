using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.RestApiClient.Model
{
    public class Booking
    {
        public int From { get; set; }
        public int Destination { get; set; }
        public DateTime DepartureDay { get; set; }
        public DateTime ReturnDay { get; set; }
        /*public int AdultTickets { get; set; }
        public int ChildrenTickets { get; set; }
        public int BabyTickets { get; set; }*/
    }
}
