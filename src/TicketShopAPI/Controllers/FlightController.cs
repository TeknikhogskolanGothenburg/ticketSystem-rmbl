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
    [Route("api/Flight")]
    public class FlightController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();

        /// <summary>
        /// querries database for all flights
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> all flights as json | StatusCode: 200 OK</returns>
        /// <returns> there are no flights | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Flight
        [HttpGet]
        public IEnumerable<string> Get(DateTime departureDate, int departurePort, DateTime arrivalDate, int arrivalPort, int seats)
        {
            List<Flight> allFlights = new List<Flight>();
            if (security.IsAuthorised("NotSureYet"))
            {
                //allFlights = TicketDb.FlightAdd(allFlights);
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
        public void FlightFind(int id)
        {
            /*if (security.IsAuthorised("NotSureYet"))
            {
                Flight flight = new Flight();
                //List<Flight> queryResult = TicketDb.FlightFind(id.ToString());
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
            }*/
        }

        [HttpGet("{id}/AvaliableSeats")]
        public List<int> GetAvaliableSeats(int id)
        {
            return TicketDb.AvaliableSeatsFind(id);
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

                try
                {
                    Flight newFlight = TicketDb.FlightAdd(flight.DepartureDate, flight.DeparturePort, flight.ArrivalDate, flight.ArrivalPort, flight.Seats);
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
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="data">flight data used to update</param>
        /// <param name="id">id of flight to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // PUT: api/Flight/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]JObject data)
        {
            if (security.IsAuthorised("NotSureYet"))
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
                    TicketDb.FlightModify(id, flight.DepartureDate, flight.DeparturePort, flight.ArrivalDate, flight.ArrivalPort, flight.Seats);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
            }
        }

        /// <summary>
        /// deletes a flight based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="id">id of flight to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // DELETE: api/Flight/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                bool deleteSuccessful = TicketDb.FlightDelete(id);
                if (!deleteSuccessful)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
            }
        }
    }
}
