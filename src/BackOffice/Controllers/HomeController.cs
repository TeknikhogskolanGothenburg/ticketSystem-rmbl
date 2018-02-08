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

        public IActionResult Login([FromBody] User user)
        {
            return null;
        }

        public IActionResult Users()
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
