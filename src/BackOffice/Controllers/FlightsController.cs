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
        /// <summary>
        /// List of Flights
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return null;
        }

        /// <summary>
        /// View a flight
        /// </summary>
        /// <param name="id">Flight index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }
    }
}