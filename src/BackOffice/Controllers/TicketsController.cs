using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.RestApiClient;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;

namespace BackOffice.Controllers
{
    public class TicketsController : Controller
    {
        private TicketApi ticketApi;

        /// <summary>
        /// Default constructor, prepare api
        /// </summary>
        public TicketsController()
        {
            ApiInformation api = new ApiInformation();

            if (TempData["Userid"] != null)
            {
                ticketApi = new TicketApi(api.Key, api.Secret, (int)TempData["SessionId"], (string)TempData["SessionSecret"]);
            }
            else
            {
                ticketApi = new TicketApi(api.Key, api.Secret);
            }
        }

        /// <summary>
        /// Get a list of buyed tickets for a user
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
    }
}