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
        [HttpGet]
        public IActionResult Index()
        {
            var model = new DataBaseRep();
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
                    sb.Append("SELECT ID, Name FROM AirPorts");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                model.Response.Add(reader.GetInt32(0), reader.GetString(1));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                model.Response.Add(1, e.ToString());
            }
            return View("Index", model);
            //return View("Index", model);
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            return null;
        }

        [HttpPost]
        public IActionResult NewBooking([FromBody] Booking booking)
        {
            return null;
        }

        [HttpPost]
        public IActionResult CheckOut([FromBody] Booking booking)
        {
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Booking(int? id)
        {
            return null;
        }

        [HttpPost("{id}")]
        public IActionResult EditBooking(int? id, [FromBody] Booking booking)
        {
            return null;
        }

        public IActionResult Profile()
        {
            return null;
        }

        [HttpPost]
        public IActionResult Settings([FromBody] User user)
        {
            return null;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
