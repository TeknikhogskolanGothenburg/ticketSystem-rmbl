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
    [Route("api/AirPort")]
    public class AirPortController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();

        /// <summary>
        /// querries database for all AirPorts
        /// </summary>       
        /// <returns> all AirPorts as json | StatusCode: 200 OK</returns>
        /// <returns> there are no AirPorts | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/AirPort
        [HttpGet]
        public IEnumerable<string> Get()
        {            
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                List<AirPort> allAirPorts = new List<AirPort>();
                allAirPorts = TicketDb.AirPortFindAll();
                if (allAirPorts.Count != 0)
                {

                    return allAirPorts.Select(u => JsonConvert.SerializeObject(u));
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return new string[] { "there are no AirPorts" };
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new string[] { "access denied" };
            }
            
        }


        /// <summary>
        /// querries database for AirPort by id
        /// </summary>        
        /// <returns> ticket as json | StatusCode: 200 OK</returns>
        /// <returns> that AirPort does not exsist | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/AirPort/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];

            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                AirPort AirPort = new AirPort();
                AirPort queryResult = TicketDb.AirPortFind(id);
                if (queryResult != null)
                {
                    AirPort = queryResult;
                    return JsonConvert.SerializeObject(AirPort);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return "that AirPort does not exsist";
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return "access denied";
            }
        }

        /// <summary>
        /// querries database for departure flights from a specific airport
        /// </summary>
        /// <param name="id">id of airport</param>
        /// <returns> List of flights as json | StatusCode: 200 OK</returns>
        /// <returns> | StatusCode: 401 Unauthorized</returns>
        // GET: api/AirPort/5/DepartureFlight
        [HttpGet("{id}/DepartureFlight")]
        public IEnumerable<string> GetDepartureFlight(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                return TicketDb.AirportDeparturesFind(id).Select(f => JsonConvert.SerializeObject(f));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return null;
            }

        }

        /// <summary>
        /// querries database for arrival flights from a specific airport
        /// </summary>
        /// <param name="id">id of airport</param>
        /// <returns> List of flights as json | StatusCode: 200 OK</returns>
        /// <returns> | StatusCode: 401 Unauthorized</returns>
        // GET: api/AirPort/5/ArrivalFlight
        [HttpGet("{id}/ArrivalFlight")]
        public IEnumerable<string> GetArrivalFligth(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                return TicketDb.AirportArrivalsFind(id).Select(f => JsonConvert.SerializeObject(f));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return null;
            }
        }

        /// <summary>
        /// Adds a new AirPort to the database
        /// </summary>        
        /// <param name="data">new AirPort data to be added to database</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        /// <returns>void | StatusCode: 409 Conflict</returns>
        // POST: api/AirPort
        [HttpPost]
        public void Post([FromBody]JObject data)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                AirPort AirPort;
                try
                {
                    AirPort = data["AirPort"].ToObject<AirPort>();
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                try
                {
                    AirPort newAirPort = TicketDb.AirPortAdd(AirPort.Name, AirPort.Country, AirPort.UTCOffset);
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
        /// updates a AirPort based on id
        /// </summary>
        /// <param name="data">AirPort data used to update</param>
        /// <param name="id">id of AirPort to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // PUT: api/AirPort/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]JObject data)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                AirPort AirPort;
                try
                {
                    AirPort = data["AirPort"].ToObject<AirPort>();
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                try
                {
                    TicketDb.AirPortModify(id, AirPort.Name, AirPort.Country, AirPort.UTCOffset);
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
        /// deletes an airport based on id
        /// </summary>       
        /// <param name="id">id of airport to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // DELETE: api/AirPort/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                bool deleteSuccessful = TicketDb.AirPortDelete(id);
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
