 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Data.SqlClient;
using TicketShop.Models;
using TicketSystem.RestApiClient.Model;
using TicketSystem.RestApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketShop.Controllers
{
    public class HomeController : Controller
    {
        private IConfigurationRoot config;
        private Sessions sessions;
        private TicketApi ticketApi;
        private MessagesHandler messagesHandler;
        public List<TicketVariables> tickets = new List<TicketVariables>();

        /// <summary>
        /// Constructor with api settings and sessions object
        /// </summary>
        public HomeController(IConfigurationRoot newConfig, Sessions newSessions)
        {
            config = newConfig;
            sessions = newSessions;
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

        public IActionResult Index()
        {
            List<AirPort> airports = ticketApi.GetAirPorts();
            ViewBag.AirPorts = airports;

            var temp = ticketApi.GetTicketById(1);

            return View(new FlightSearch());
        }

        
        public ActionResult Booking(FlightSearch flightSearch)
        {
            List<AirPort> airports = ticketApi.GetAirPorts();
            List<Flight> flights = ticketApi.GetFlights(); 
            List<TicketVariables> tickets = new List<TicketVariables>();

            foreach (Flight x in flights)
            {
                if(x.DeparturePort == flightSearch.From)
                {
                    if(x.ArrivalPort == flightSearch.Destination)
                    {
                        if(x.DepartureDate == flightSearch.DepartureDay)
                        {
                            tickets.Add(
                            new TicketVariables
                            {
                                From = airports.ElementAt(x.DeparturePort-1).Name,
                                To = airports.ElementAt(x.ArrivalPort-1).Name,
                                SeatNum = 16,
                                Departure = x.DepartureDate,
                                Arrival = x.ArrivalDate,
                                Price = x.Price,
                            }   
                            );
                        }
                    }
                }
            }

            if (tickets.Count > 0)
            {
                return View("Booking", tickets);
            }
            return View("_NoFlights");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
