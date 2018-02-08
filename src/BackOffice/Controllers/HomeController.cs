using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;

namespace BackOffice.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            return null;
        }

        public IActionResult Users()
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult User(int? id)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult EditUser(int? id)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult RemoveUser(int? id)
        {
            return null;
        }

        [HttpGet("{userId}")]
        public IActionResult Bookings(int? userId)
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

        [HttpPost("{id}")]
        public IActionResult RemoveBooking(int? id, [FromBody] Booking booking)
        {
            return null;
        }

        public IActionResult Flights()
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Flight(int? id)
        {
            return null;
        }

        public IActionResult Franshieses()
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Franshies(int? id)
        {
            return null;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
