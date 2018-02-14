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
    [Route("api/Ticket")]
    public class TicketController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();

        /// <summary>
        /// querries database for all tickets
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> all tickets as json | StatusCode: 200 OK</returns>
        /// <returns> there are no tickts | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Ticket
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<Ticket> allTickets = new List<Ticket>();
            if (security.IsAuthorised("NotSureYet"))
            {
                allTickets = TicketDb.TicketFind("");
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new string[] { "access denied" };
            }
            if (allTickets.Count != 0)
            {

                return allTickets.Select(u => JsonConvert.SerializeObject(u));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NoContent;
                return new string[] { "there are no tickts" };
            }
        }

        /// <summary>
        /// querries database for ticket by id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> ticket as json | StatusCode: 200 OK</returns>
        /// <returns> that ticket does not exsist | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                Ticket ticket = new Ticket();
                List<Ticket> queryResult = TicketDb.TicketFind(id.ToString());
                if (queryResult.Count > 0)
                {
                    ticket = queryResult[0];
                    return JsonConvert.SerializeObject(ticket);
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
        /// Adds a new ticket to the database
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="data">data used to process purchase of a ticket</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 402 PaymentRequired</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        /// <returns>void | StatusCode: 409 Conflict</returns>
        // POST: api/Ticket
        [HttpPost]
        public void Post([FromBody]JObject data)
        {
            if (security.IsAuthorised(Request.Headers["Authorization"]))
            {
                Ticket ticket;
                try
                {
                    ticket = data["Ticket"].ToObject<Ticket>();
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
                Payment payment = data["payment"].ToObject<Payment>();
                //is the seat already taken?
                foreach (Ticket t in TicketDb.TicketFind(""))
                {
                    if (t.FlightID == ticket.FlightID && t.SeatNumber == ticket.SeatNumber)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                        return;
                    }
                }

                PaymentProvider paymentProvider = new PaymentProvider();
                Payment paymentAttempt = paymentProvider.Pay(payment.TotalAmount, payment.Valuta, payment.OrderReference);
                Transaction newTransaction = TicketDb.TransactionAdd(paymentAttempt.PaymentStatus.ToString(), paymentAttempt.PaymentReference);
                if (paymentAttempt.PaymentStatus == PaymentStatus.PaymentRejected || paymentAttempt.PaymentStatus == PaymentStatus.UnknownError)
                {
                    Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
                    return;
                }
                try
                {
                    Ticket newTicket = TicketDb.TicketAdd(ticket.UserID, ticket.FlightID, ticket.SeatNumber, ticket.BookAt);
                    TicketDb.TicketToTransactionAdd(newTicket.ID, newTransaction.ID);
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
        /// updates a user based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="ticket">ticket data used to update</param>
        /// <param name="id">id of ticket to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Ticket ticket)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                if (ticket == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                Ticket updatedTicket = TicketDb.TicketModify(id, ticket.UserID, ticket.FlightID, ticket.SeatNumber, ticket.BookAt);
                if (updatedTicket == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }

        /// <summary>
        /// deletes a ticket based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="id">id of ticket to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        /// <returns>void | StatusCode: 501 NotImplemented</returns>
        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {            
            Response.StatusCode = (int)HttpStatusCode.NotImplemented;
            return;
            if (security.IsAuthorised("NotSureYet"))
            {
                try
                {
                    TicketDb.TicketDelete(id);
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
    }
}
