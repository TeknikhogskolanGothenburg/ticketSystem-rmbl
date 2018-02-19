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
            return View(new FlightSearch());
        }

        public IActionResult RegisterUser(User user)
        {

            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }


        public ActionResult Booking(FlightSearch flightSearch)
        {
            List<TicketVariables> tickets = new List<TicketVariables>();

            try
            {
                List<Flight> flightsAirportDate = ticketApi.GetFlightsByAirportDate(flightSearch.From, flightSearch.DepartureDay.ToString("yyyyMMdd"));
                List<Flight> customersChoice = new List<Flight>();

                string airPortFromName = "";
                string airPortDestinationName = "";

                foreach (AirPort airport in ticketApi.GetAirPorts())
                {
                    if (airport.ID == flightSearch.From)
                    {
                        airPortFromName = airport.Name;
                    }
                    if (airport.ID == flightSearch.Destination)
                    {
                        airPortDestinationName = airport.Name;
                    }
                }

                int seatNumber = 0;


                foreach (Flight flight in flightsAirportDate)
                {
                    if (flight.ArrivalPort == flightSearch.Destination)
                    {
                        customersChoice.Add(flight);
                    }
                }

                foreach (Flight flight in customersChoice)
                {
                    seatNumber = ticketApi.GetFlightSeats(flight.Id)[0];
                    tickets.Add(new TicketVariables
                    {
                        From = airPortFromName,
                        To = airPortDestinationName,
                        SeatNum = seatNumber,
                        Departure = flight.DepartureDate,
                        Arrival = flight.ArrivalDate,
                        Price = flight.Price
                    });
                }

                if (tickets.Count > 0)
                {
                    return View("Booking", tickets);
                }
            }
            catch (Exception ex)
            {
                messagesHandler.Add("danger", ex.Message);
            }
            messagesHandler.Add("warning", "No flights available");

            return RedirectToAction("index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
