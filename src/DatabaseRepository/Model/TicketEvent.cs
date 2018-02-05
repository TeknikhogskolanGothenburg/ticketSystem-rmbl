using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseRepository.Model
{
    public class TicketEvent
    {
        public int TicketEventId { get; set; }
        public string EventName { get; set; }
        public string EventHtmlDescription { get; set; }
    }
}
