using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class Flight
    {
        [DataType(DataType.Date)]
        [Display(Name = "Departure time")]
        public DateTime DepartureDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Departure Airport")]
        public int DeparturePort { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Arrival time")]
        public DateTime ArrivalDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Arrival Airport")]
        public int ArrivalPort { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Seats { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Ticket price")]
        public int Price { get; set; }
    }
}
