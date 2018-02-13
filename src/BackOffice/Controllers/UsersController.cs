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
    public class UsersController : Controller
    {
        private TicketApi ticketApi;

        /// <summary>
        /// Default constructor, prepare api
        /// </summary>
        public UsersController()
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
        /// List of users
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View user
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }

        /// <summary>
        /// Edit user, without post
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Edit(int? id)
        {
            return null;
        }

        /// <summary>
        /// Edit user, with post
        /// </summary>
        /// <param name="id">User index</param>
        /// <param name="user">User object with post information</param>
        /// <returns>View or redirect</returns>
        [HttpPost("{id}")]
        public IActionResult Edit(int? id, User user)
        {
            return null;
        }

        /// <summary>
        /// Remove user, without post
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Remove(int? id)
        {
            return null;
        }

        /// <summary>
        /// Remove user, with post
        /// </summary>
        /// <param name="id">User index</param>
        /// <param name="remove">Bool-value as string</param>
        /// <returns>Redirect</returns>
        [HttpPost("{id}")]
        public IActionResult Remove(int? id, string remove = "false")
        {
            return null;
        }
    }
}