namespace TicketSystem.RestApiClient.Model
{
    public class Ticket
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double UTCOffset { get; set; }
    }
}
