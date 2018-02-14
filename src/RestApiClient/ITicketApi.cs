using System.Collections.Generic;
using TicketSystem.RestApiClient.Model;

namespace TicketSystem.RestApiClient
{
    public interface ITicketApi
    {
        /// <summary>
        /// Get all tickets, from one user
        /// </summary>
        /// <returns>List with Ticket objects</returns>
        List<Ticket> GetTicketsByUser(int userId);

        /// <summary>
        /// Get a ticket by ID from the system Returns a single ticket
        /// </summary>
        /// <param name="ticketId">ID of the ticket</param>
        /// <returns>Ticket object</returns>
        Ticket GetTicketById(int ticketId);
    }
}
