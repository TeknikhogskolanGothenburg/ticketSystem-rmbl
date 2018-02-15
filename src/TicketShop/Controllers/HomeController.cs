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

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
