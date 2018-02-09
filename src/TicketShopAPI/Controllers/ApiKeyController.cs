using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using TicketSystem.DatabaseRepository.Model;

namespace TicketShopAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ApiKey")]
    public class ApiKeyController : Controller
    {
        // GET: api/ApiKey
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ApiKey/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ApiKey
        [HttpPost]
        public ApiKeys Post([FromBody]Franchise franchise)
        {
            TicketDatabase ticketdb = new TicketDatabase();

            ApiKeys newKey = GenerateApiKey();
            ticketdb.FranchiseAdd(franchise.Name);
            ticketdb.ApiKeyAdd(newKey.KeyValue, newKey.Secret);
            return newKey;
        }

        ///<summary>
        ///This is where we are going to generate your unique api key
        ///</summary>
        private ApiKeys GenerateApiKey()
        {
            throw new NotImplementedException();
        }

        // PUT: api/ApiKey/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
