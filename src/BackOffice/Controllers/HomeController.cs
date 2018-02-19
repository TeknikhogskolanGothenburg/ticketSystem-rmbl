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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BackOffice.Controllers
{
    public class HomeController : Controller
    {
        private IConfigurationRoot config;
        private Sessions sessions;
        private TicketApi ticketApi;
        private MessagesHandler messagesHandler;
        private ILogger<HomeController> logger;

        /// <summary>
        /// Constructor sith logging, sessions and app settings
        /// </summary>
        /// <param name="newLogger">Logger</param>
        /// <param name="newConfig">App settings</param>
        /// <param name="newSessions">Sessions</param>
        public HomeController(ILogger<HomeController> newLogger, IConfigurationRoot newConfig, Sessions newSessions)
        {
            config = newConfig;
            sessions = newSessions;
            logger = newLogger;
        }

        /// <summary>
        /// Before actions are executing, do this... 
        /// </summary>
        /// <param name="context">Action context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sessions.Intialize(context.HttpContext);
            ViewData["Sessions"] = sessions;

            if (sessions.Get("UserId") != null)
            {
                ticketApi = new TicketApi(config["Api:Key"], config["Api:Secret"], (int)sessions.Get("SessionId"), (string)sessions.Get("SessionSecret"));
            }
            else
            {
                ticketApi = new TicketApi(config["Api:Key"], config["Api:Secret"]);
            }

            messagesHandler = new MessagesHandler(sessions);

            ViewData["Messages"] = messagesHandler;

            // Execute action
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Index and login page without post
        /// </summary>
        /// <returns>Login view</returns>
        public IActionResult Index()
        {
            if(!sessions.Exist("UserId"))
            {
                return View(new Login());
            }
            
            messagesHandler.Add("warning", "You are already login!");

            return RedirectToAction("Index", "Users");
        }

        /// <summary>
        /// Index and login with post, try to login through WebApi
        /// </summary>
        /// <param name="login">Login information</param>
        /// <returns>Login view or redirect</returns>
        [HttpPost]
        public IActionResult Index(Login login)
        {
            if (!sessions.Exist("UserId"))
            {
                if (login == null)
                {
                    return View(new Login());
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        SessionInfo sessionInfo = ticketApi.PostLoginIn(login);

                        sessions.Add("Userid", sessionInfo.UserId);
                        sessions.Add("Username", sessionInfo.Username);
                        sessions.Add("SessionId", sessionInfo.SessionId);
                        sessions.Add("SessionSecret", sessionInfo.SessionSecret);

                        messagesHandler.Add("primary", "Welcome " + sessionInfo.Username + "!");      

                        return RedirectToAction("Index", "Users");
                    }   
                    catch (Exception ex)
                    {
                        messagesHandler.Add("danger", ex.Message);
                    }
                }

                return View(login);
            }

            messagesHandler.Add("warning", "You are already login!");

            return RedirectToAction("Index", "Users");
        }

        /// <summary>
        /// Add flight, without post
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult AddFlight()
        {
            if (sessions.Exist("UserId"))
            {
                List<AirPort> airports = ticketApi.GetAirPorts();
                ViewBag.AirPorts = airports;

                return View(new Flight());
            }

            messagesHandler.Add("warning", "You need to be login, for add fligth!");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Add fligth, with post
        /// </summary>
        /// <param name="flight">Post values in flight object</param>
        /// <returns>Redirect or view</returns>
        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            if (sessions.Exist("UserId"))
            {
                if (flight != null)
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

            messagesHandler.Add("warning", "You need to be login, for add fligth!");

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}