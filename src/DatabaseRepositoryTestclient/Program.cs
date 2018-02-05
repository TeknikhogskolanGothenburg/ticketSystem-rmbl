using DatabaseRepository;
using System;

namespace DatabaseRepositoryTestclient
{
    class Program
    {
        static void Main(string[] args)
        {
            ITicketDatabase ticketDatabase = new TicketDatabase();
            var tevent = ticketDatabase.EventAdd("Event1", "Some desciption");
            Console.WriteLine("TicketEventId: " + tevent.TicketEventId);
            var venue = ticketDatabase.VenueAdd("venue1", "Some address", "city1","country1");
            Console.WriteLine("VenueId: " + venue.VenueId);

            var venues1 = ticketDatabase.VenuesFind("ven");
            foreach (var ven in venues1)
            {
                Console.WriteLine("Find(ven) - VenueId: " + ven.VenueId);
            }

            var venues2 = ticketDatabase.VenuesFind("y1");
            foreach (var ven in venues2)
            {
                Console.WriteLine("Find(y1) - VenueId: " + ven.VenueId);
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
