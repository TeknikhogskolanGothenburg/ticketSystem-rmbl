﻿using System;
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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketShop.Controllers
{
    public class UserController : Controller
    {
        private TicketApi ticketApi;
        public UserController()
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
        // GET: /<controller>/
        [HttpGet("{id}")]
        public ActionResult Index(string id)
        {
            int x;
            if (int.TryParse(id, out x) == true)
            {
                ticketApi.GetTicketById(x);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
