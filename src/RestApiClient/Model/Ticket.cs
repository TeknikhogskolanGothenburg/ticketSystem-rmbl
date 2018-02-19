using System.ComponentModel.DataAnnotations;

namespace TicketSystem.RestApiClient.Model
{
    public class Ticket
    {
        public int ID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int FlightID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SeatNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserID { get; set; }

        public int BookAt { get; set; }
    }
}
