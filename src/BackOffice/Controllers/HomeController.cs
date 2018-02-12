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
        /// <summary>
        /// Back office start page
        /// </summary>
        /// <returns>Start page view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Login page without post
        /// </summary>
        /// <returns>Login view</returns>
        public IActionResult Login()
        {
            return View(new Login());
        }

        /// <summary>
        /// Login with post, try to login through WebApi
        /// </summary>
        /// <param name="login">Login information</param>
        /// <returns>Login view or redirect</returns>
        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (login != null)
            {

            }

            return View(login);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
