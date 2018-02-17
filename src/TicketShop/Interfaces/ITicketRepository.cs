using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketShop.Models;

namespace TicketShop.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> Tickets { get; }

        Ticket GetTicketById(int ticketId);
    }
}
