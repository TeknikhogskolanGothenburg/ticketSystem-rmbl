using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;

namespace BackOffice.Controllers
{
    public class TicketsController : Controller
    {
        [HttpGet("{userId}")]
        public IActionResult Index(int? userId)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int? id)
        {
            return null;
        }

        [HttpPost("{id}")]
        public IActionResult Edit(int? id, Booking booking)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Remove(int? id)
        {
            return null;
        }

        [HttpPost("{id}")]
        public IActionResult Remove(int? id, string remove = "false")
        {
            return null;
        }
    }
}