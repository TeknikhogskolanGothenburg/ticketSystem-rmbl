using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using TicketSystem.DatabaseRepository.Model;
using Newtonsoft.Json;
using TicketSystem.PaymentProvider;
using Newtonsoft.Json.Linq;
using TicketShopAPI.APISecurity;

namespace TicketShopAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Franchise")]
    public class FranchiseController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();

        //// GET: api/Franchise
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Franchise/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("{id}/Key")]
        public string GetKey(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(apiKeyData, sessionData, gradeRestriction))
            {
                return TicketDb.APIKeyFind(id);
            }
            else
            {
                return "access denied";
            }
        }

        //// POST: api/Franchise
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Franchise/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
