using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class Transaction
    {
        public int ID { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentReferenceId { get; set; }
    }
}
