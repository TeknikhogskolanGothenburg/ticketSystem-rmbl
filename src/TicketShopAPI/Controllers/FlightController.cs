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
    [Route("api/Flight")]
    public class FlightController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();
        private ILogger<FlightController> logger;

        public FlightController(ILogger<FlightController> newLogger)
        {
            logger = newLogger;
        }

        /// <summary>
        /// querries database for all flights
        /// </summary>
        /// <returns> all flights as json | StatusCode: 200 OK</returns>
        /// <returns> there are no flights | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Flight
        [HttpGet]
        public IEnumerable<string> Get()
        {

            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = -1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                List<Flight> allFlights = new List<Flight>();
                allFlights = TicketDb.FlightFindAll();
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
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new string[] { "access denied" };
            }
            
        }


        /// <summary>
        /// querries database for flight by id
        /// </summary>
        /// <returns> ticket as json | StatusCode: 200 OK</returns>
        /// <returns> that flight does not exsist | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Flight/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = -1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                Flight flight = TicketDb.FlightFind(id);
                if (flight != null)
                {
                    return JsonConvert.SerializeObject(flight);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return "that flight does not exsist";
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return "access denied";
            }
        }

        /// <summary>
        /// querries database for all avaliable seat numbers on a particular flight
        /// </summary>
        /// <returns> a list of set numbers | StatusCode: 200 OK</returns>
        /// <returns> that flight does not exsist | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Flight/5/AvaliableSeats
        [HttpGet("{id}/AvaliableSeats")]
        public List<int> GetAvaliableSeats(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = -1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                Flight flight = TicketDb.FlightFind(id);
                if (flight != null)
                {
                    return TicketDb.AvaliableSeatsFind(id);
                }
                Response.StatusCode = (int)HttpStatusCode.NoContent;
                return new List<int>();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new List<int>();
            }
        }

        /// <summary>
        /// Adds a new Flight to the database
        /// </summary>
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
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
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

                try
                {
                    Flight newFlight = TicketDb.FlightAdd(flight.DepartureDate, flight.DeparturePort, flight.ArrivalDate, flight.ArrivalPort, flight.Seats, flight.Price);
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

        /// <summary>
        /// updates a Flight based on id
        /// </summary>
        /// <param name="data">flight data used to update</param>
        /// <param name="id">id of flight to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // PUT: api/Flight/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]JObject data)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
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

                try
                {
                    TicketDb.FlightModify(id, flight.DepartureDate, flight.DeparturePort, flight.ArrivalDate, flight.ArrivalPort, flight.Seats, flight.Price);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }

        /// <summary>
        /// deletes a flight based on id
        /// </summary>
        /// <param name="id">id of flight to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // DELETE: api/Flight/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                bool deleteSuccessful = TicketDb.FlightDelete(id);
                if (!deleteSuccessful)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }
    }
}
