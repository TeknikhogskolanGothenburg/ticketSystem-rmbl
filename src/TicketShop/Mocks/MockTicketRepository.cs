using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketShop.Interfaces;
using TicketShop.Models;

namespace TicketShop.Mocks
{
    public class MockTicketRepository:ITicketRepository
    {
        private readonly ITicketRepository _ticketRepository = new MockTicketRepository();
        public IEnumerable<Ticket> Tickets {
            get
            {
                return new List<Ticket>
                {
                    new Ticket
                    {
                        From = "Venecia",
                        To = "Japan",
                        SeatNum = 16,
                        Departure = DateTime.Now,
                        Price = 10000,
                    },

                    new Ticket
                    {
                        From = "Venecia",
                        To = "Singapore",
                        SeatNum = 17,
                        Departure = DateTime.Now,
                        Price = 10000,
                    },

                    new Ticket
                    {
                        From = "Venecia",
                        To = "Madrid",
                        SeatNum = 18,
                        Departure = DateTime.Now,
                        Price = 10000,
                    },

                    new Ticket
                    {
                        From = "Venecia",
                        To = "Barcelona",
                        SeatNum = 19,
                        Departure = DateTime.Now,
                        Price = 10000,
                    },

                    new Ticket
                    {
                        From = "Venecia",
                        To = "Moskva",
                        SeatNum = 20,
                        Departure = DateTime.Now,
                        Price = 10000,
                    },
                };
            }
        }

        public Ticket GetTicketById(int ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
