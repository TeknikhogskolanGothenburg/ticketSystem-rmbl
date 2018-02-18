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
            List<AirPort> airports = new List<AirPort>
            {
                new AirPort{ID = 1, Name = "Madrid", Country = "ES", UTCOffset = 1.10 },
                new AirPort{ID = 2, Name = "Barcelona", Country = "ES", UTCOffset = 1.10 },
                new AirPort{ID = 3, Name = "Venecia", Country = "IT", UTCOffset = 1.20 },
                new AirPort{ID = 4, Name = "Japan", Country = "JP", UTCOffset = 1.30 },
                new AirPort{ID = 5, Name = "Moskva", Country = "RU", UTCOffset = 1.40 }
            };

            /*try
            {
                airports = ticketApi.GetAirPorts();
            }
            catch (Exception ex)
            {
                messagesHandler.Add("danger", ex.Message);
                airports = new List<AirPort>();
            }*/

            ViewBag.AirPorts = airports;

            return View(new FlightSearch());
        }

        
        public ActionResult Booking()
        {
            var model = new TicketVariables();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "rmbl.database.windows.net";
                builder.UserID = "rmblA";
                builder.Password = "QAwsedrf123@@";
                builder.InitialCatalog = "RMBL-SERVER";
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    //Console.WriteLine("Opening the port");
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT A.Name, B.Name, DepatureDate, ArrivalDate, Price FROM Flights LEFT JOIN AirPorts AS A ON A.ID = Flights.DeparturePort INNER JOIN AirPorts AS B ON B.ID = Flights.ArrivalPort");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var temp = reader.GetString(0);
                                tickets.Add(
                                    new TicketVariables {
                                        From = reader.GetString(0),
                                        To = reader.GetString(1),
                                        Departure = reader.GetDateTime(2),
                                        Arrival = reader.GetDateTime(3),
                                        Price = reader.GetInt32(4)
                                    });
                            }
                        }

                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
               
            }
            

            return View("Booking", tickets);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
