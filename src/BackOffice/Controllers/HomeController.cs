using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.RestApiClient;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;
using Microsoft.AspNetCore.Session;

namespace BackOffice.Controllers
{
    public class HomeController : Controller
    {
        private TicketApi ticketApi;

        /// <summary>
        /// Default constructor, prepare api
        /// </summary>
        public HomeController()
        {
            ApiInformation api = new ApiInformation();

            if ((TempData != null) && TempData["Userid"] != null)
            {
                ticketApi = new TicketApi(api.Key, api.Secret, (int)TempData["SessionId"], (string)TempData["SessionSecret"]);
            }
            else
            {
                ticketApi = new TicketApi(api.Key, api.Secret);
            }
        }

        /// <summary>
        /// Index and login page without post
        /// </summary>
        /// <returns>Login view</returns>
        public IActionResult Index()
        {
            return View(new Login());
        }

        /// <summary>
        /// Index and login with post, try to login through WebApi
        /// </summary>
        /// <param name="login">Login information</param>
        /// <returns>Login view or redirect</returns>
        [HttpPost]
        public IActionResult Index(Login login)
        {
            if (login != null && ModelState.IsValid)
            {
                /*try
                {
                    SessionInfo sessionInfo = ticketApi.PostLoginIn(login);

                    TempData.Add("Userid", sessionInfo.UserId);
                    TempData.Add("Username", sessionInfo.Username);
                    TempData.Add("SessionId", sessionInfo.SessionId);
                    TempData.Add("SessionSecret", sessionInfo.SessionSecret);
                }
                catch (Exception ex)
                {
                    
                }*/
            }

            return View(login);
        }

        public IActionResult AddFlight()
        {
            return View(new Flight());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
