using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Data.SqlClient;
using TicketShop.Models;
using TicketSystem.RestApiClient.Model;
using TicketSystem.RestApiClient;

namespace TicketShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            return null;
        }

        [HttpPost]
        public IActionResult NewBooking([FromBody] Booking booking)
        {
            return null;
        }

        [HttpPost]
        public IActionResult CheckOut([FromBody] Booking booking)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Booking(int? id)
        {
            return null;
        }

        [HttpPost("{id}")]
        public IActionResult EditBooking(int? id, [FromBody] Booking booking)
        {
            return null;
        }

        public IActionResult Profile()
        {
            return null;
        }

        [HttpPost]
        public IActionResult Settings([FromBody] User user)
        {
            return null;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public ActionResult Booking()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public ActionResult _Ticket(TicketVariables temp, string abb, string firstName, string lastName, string from, string to, int seatNum, DateTime dep, DateTime arrival, int price)
        {
            var model = new TicketVariables
            {
                Abbriviation = abb,
                FirstName = firstName,
                LastName = lastName,
                From = from,
                To = to,
                SeatNum = seatNum,
                Departure = dep,
                Arrival = arrival,
                Price = price,
            };

            var testModel = new TicketVariables
            {
                Abbriviation = "Lord",
                FirstName = "Z",
                LastName = "",
                From = "Venecia",
                To = "Dubai",
                SeatNum = 16,
                Departure = DateTime.Now,
                Arrival = DateTime.Today,
                Price = 10000,
            };
            return PartialView("_Ticket", testModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
