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
    public class FranshiesesController : Controller
    {
        private TicketApi ticketApi;

        /// <summary>
        /// Default constructor, prepare api
        /// </summary>
        public FranshiesesController()
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
        /// List of existing franshieses
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View a franshies
        /// </summary>
        /// <param name="id">Franshies index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }
    }
}