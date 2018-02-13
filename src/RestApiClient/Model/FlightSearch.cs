using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    class FlightSearch
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int From { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Departure day")]
        public DateTime DepartureDay { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return day")]
        public DateTime ReturnDay { get; set; }
    }
}
