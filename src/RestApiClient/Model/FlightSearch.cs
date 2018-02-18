using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class FlightSearch
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int From { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Departure day")]
        public DateTime DepartureDay { get; set; }
    }
}
