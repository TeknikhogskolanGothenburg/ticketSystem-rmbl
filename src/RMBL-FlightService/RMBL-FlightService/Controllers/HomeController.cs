using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Web.Mvc;
using RMBL_FlightService.Models;

namespace RMBL_FlightService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SQL()
        {
            var model = new DataBaseResp();
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
                    sb.Append("SELECT * FROM AirPorts");
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
            return View("SQL", model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}