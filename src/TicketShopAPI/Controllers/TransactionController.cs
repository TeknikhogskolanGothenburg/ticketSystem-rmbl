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
using Microsoft.Extensions.Logging;

namespace TicketShopAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Transaction")]
    public class TransactionController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();
        private ILogger<TransactionController> logger;

        public TransactionController(ILogger<TransactionController> newLogger)
        {
            logger = newLogger;
        }


        // GET: api/Transaction
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                List<Transaction> allTransactions = TicketDb.TransactionFind();
                return allTransactions.Select(t => JsonConvert.SerializeObject(t));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return null;
            }
        }

        //// GET: api/Transaction/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return null;
        //}

        //// POST: api/Transaction
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Transaction/5
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
