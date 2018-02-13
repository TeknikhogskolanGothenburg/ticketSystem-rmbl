using System.ComponentModel.DataAnnotations;

namespace TicketSystem.RestApiClient.Model
{
    public class Ticket
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double UTCOffset { get; set; }
        public int TicketId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int FlightID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SeatNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BookAt { get; set; }
    }
}
