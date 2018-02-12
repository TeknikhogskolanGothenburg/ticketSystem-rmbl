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
        /// <summary>
        /// Get a list of buy tickets for a user
        /// </summary>
        /// <param name="userId">User index</param>
        /// <returns>View</returns>
        [HttpGet("{userId}")]
        public IActionResult Index(int? userId)
        {
            return null;
        }

        /// <summary>
        /// Show a ticket
        /// </summary>
        /// <param name="id">Ticket index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }

        /// <summary>
        /// Edit a ticket, without post
        /// </summary>
        /// <param name="id">Ticket index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Edit(int? id)
        {
            return null;
        }

        /// <summary>
        /// Edit a ticket, with post
        /// </summary>
        /// <param name="id">Ticket index</param>
        /// <param name="booking">Booking object, with post-data</param>
        /// <returns>View or redirect</returns>
        [HttpPost("{id}")]
        public IActionResult Edit(int? id, Booking booking)
        {
            return null;
        }

        /// <summary>
        /// Remove a ticket, without post
        /// </summary>
        /// <param name="id">Ticket index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Remove(int? id)
        {
            return null;
        }

        /// <summary>
        /// Remove a ticket, with post
        /// </summary>
        /// <param name="id">Ticket index</param>
        /// <param name="remove">Bool-value as string</param>
        /// <returns>Redirect</returns>
        [HttpPost("{id}")]
        public IActionResult Remove(int? id, string remove = "false")
        {
            return null;
        }
    }
}