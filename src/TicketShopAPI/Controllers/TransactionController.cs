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
    [Route("api/Transaction")]
    public class TransactionController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();

        // GET: api/Transaction
        [HttpGet]
        public IEnumerable<string> Get()
        {
            if (security.IsAuthorised(""))
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
