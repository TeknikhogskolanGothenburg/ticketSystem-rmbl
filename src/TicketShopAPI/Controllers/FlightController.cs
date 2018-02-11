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

namespace TicketShopAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Flight")]
    public class FlightController : Controller
    {
        private Security security = new Security();

        /// <summary>
        /// querries database for all flights
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> all flights as json | StatusCode: 200 OK</returns>
        /// <returns> there are no flights | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Flight
        [HttpGet]
        public IEnumerable<string> Get()
        {            
            List<Flight> allFlights = new List<Flight>();
            if (security.IsAuthorised("NotSureYet"))
            {
                TicketDatabase ticketDb = new TicketDatabase();
                allFlights = ticketDb.FlightFind("");
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new string[] { "access denied" };
            }
            if (allFlights.Count != 0)
            {

                return allFlights.Select(u => JsonConvert.SerializeObject(u));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NoContent;
                return new string[] { "there are no flights" };
            }
        }


        /// <summary>
        /// querries database for flight by id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> ticket as json | StatusCode: 200 OK</returns>
        /// <returns> that flight does not exsist | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Flight/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                Flight flight = new Flight();
                TicketDatabase ticketDb = new TicketDatabase();
                List<Flight> queryResult = ticketDb.FlightFind(id.ToString());
                if (queryResult.Count > 0)
                {
                    flight = queryResult[0];
                    return JsonConvert.SerializeObject(flight);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return "that ticket does not exsist";
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return "access denied";
            }
        }

        /// <summary>
        /// Adds a new Flight to the database
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="data">new flight data to be added to database</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 402 PaymentRequired</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        /// <returns>void | StatusCode: 409 Conflict</returns>
        // POST: api/Flight
        [HttpPost]
        public void Post([FromBody]JObject data)
        {
            if (security.IsAuthorised(Request.Headers["Authorization"]))
            {
                Flight flight;
                try
                {
                    flight = data["Flight"].ToObject<Flight>();
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                TicketDatabase ticketDb = new TicketDatabase();

                try
                {
                    Flight newFlight = ticketDb.FlightAdd();
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }
        
        // PUT: api/Flight/5
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
