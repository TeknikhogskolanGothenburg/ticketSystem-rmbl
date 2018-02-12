using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class FranshiesesController : Controller
    {
        /// <summary>
        /// List of existing franshieses
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View a franshies
        /// </summary>
        /// <param name="id">Franshies index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult View(int? id)
        {
            return null;
        }
    }
}