using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class Payment
    {
        public string Valuta { get; set; }
        public string OrderReference { get; set; }
    }
}
