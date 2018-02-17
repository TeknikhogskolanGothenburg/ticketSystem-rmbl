using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketShop.Models
{
    public class TicketVariables
    {
        public string Abbriviation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int SeatNum { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        //Maybe another variable for BARCODE
        public int Price { get; set; }

        /*public string Abbriviation = "Lord";
        public string FirstName = "Z";
        public string LastName = "";
        public string From = "Venecia";
        public string To = "Dubai";
        public int SeatNum = 16;
        public DateTime Departure = DateTime.Now;
        public DateTime Arrival = DateTime.Today;
        //Maybe another variable for BARCODE
        public int Price = 10000;*/
    }
}
