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
    public class UserController : Controller
    {
        
        public IActionResult GetAllTickets()
        {
            return View();
        }

    }
}