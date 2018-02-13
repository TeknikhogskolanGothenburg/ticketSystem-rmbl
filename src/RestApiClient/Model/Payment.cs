using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystem.RestApiClient.Model
{
    public class Payment
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression("^[A-Z]+$", ErrorMessage = "Only currency in select are accepted")]
        public string Valuta { get; set; }
        public string OrderReference { get; private set; }

        /// <summary>
        /// Default constructor, make a order reference random 
        /// </summary>
        public Payment()
        {
            OrderReference = new Guid().ToString();
        }
    }
}
