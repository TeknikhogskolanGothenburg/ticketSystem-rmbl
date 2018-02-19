using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketShop.Models
{
    public class TicketVariables
    {
        public int TicketID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int SeatNum { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int Price { get; set; }

        //public Dictionary<int, string> Response = new Dictionary<int, string>();


        /*public IEnumerable<Ticket> Tickets
        {
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
        */
    }
}
