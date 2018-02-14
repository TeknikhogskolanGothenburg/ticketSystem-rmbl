using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.RestApiClient.Model
{
    public class Booking
    {
        public Ticket Ticket { get; set; }
        public Payment Payment { get; set; }
    }
}