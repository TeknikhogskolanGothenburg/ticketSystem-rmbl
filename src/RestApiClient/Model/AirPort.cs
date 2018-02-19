using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class AirPort
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double UTCOffset { get; set; }
    }
}
