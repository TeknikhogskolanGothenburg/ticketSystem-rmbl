using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class FranshiesesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }
    }
}