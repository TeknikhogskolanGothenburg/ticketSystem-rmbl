using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;

namespace BackOffice.Controllers
{
    public class FlightsController : Controller
    {

        public IActionResult Index()
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }
    }
}