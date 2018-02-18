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

namespace TicketShop.Controllers
{
    public class HomeController : Controller
    {
        private TicketApi ticketApi;
        public List<TicketVariables> tickets = new List<TicketVariables>();


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

        public IActionResult Index()
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
                                    new TicketVariables
                                    {
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
            return View("Index", tickets);
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
