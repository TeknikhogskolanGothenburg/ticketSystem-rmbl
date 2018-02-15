using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.RestApiClient;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackOffice.Controllers
{
    public class HomeController : Controller
    {
        private TicketApi ticketApi;
        private ApiInformation api;
        private MessagesHandler messagesHandler;

        /// <summary>
        /// Default constructor, prepare api
        /// </summary>
        public HomeController()
        {
            api = new ApiInformation();
        }

        /// <summary>
        /// Before actions are executing, do this... 
        /// </summary>
        /// <param name="context">Action context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (TempData["Userid"] != null)
            {
                ticketApi = new TicketApi(api.Key, api.Secret, (int)TempData["SessionId"], (string)TempData["SessionSecret"]);
            }
            else
            {
                ticketApi = new TicketApi(api.Key, api.Secret);
            }

            messagesHandler = new MessagesHandler(TempData);

            // Execute action
            base.OnActionExecuting(context);
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
            if(login != null)
            {
                return View(new Login());
            }

            if (ModelState.IsValid)
            {
                /*try
                {
                    SessionInfo sessionInfo = ticketApi.PostLoginIn(login);

                    TempData.Add("Userid", sessionInfo.UserId);
                    TempData.Add("Username", sessionInfo.Username);
                    TempData.Add("SessionId", sessionInfo.SessionId);
                    TempData.Add("SessionSecret", sessionInfo.SessionSecret);

                    messagesHandler.Add("primary", "Welcome " + sessionInfo.Username + "!");      

                    return RedirectToAction("Index", "Users");
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                }*/
            }

            return View(login);
        }

        /// <summary>
        /// Add flight, without post
        /// </summary>
        /// <returns>View</returns>
        [HttpGet("Flights/")]
        public IActionResult AddFlight()
        {
            return View(new Flight());
        }

        /// <summary>
        /// Add fligth, with post
        /// </summary>
        /// <param name="flight">Post values in flight object</param>
        /// <returns>Redirect or view</returns>
        [HttpPost("Flights/")]
        public IActionResult AddFlight(Flight flight)
        {
            if(flight != null)
            {
                View(new Flight());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int id = ticketApi.PostFlight(flight);
                    messagesHandler.Add("success", "Flight was added successfully!");
                    return RedirectToAction("AddFlight");
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                }
            }

            return View(flight);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}