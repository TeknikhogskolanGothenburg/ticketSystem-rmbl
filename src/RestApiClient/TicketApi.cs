using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using TicketSystem.RestApiClient.Model;

namespace TicketSystem.RestApiClient
{
    public class TicketApi : ITicketApi
    {
        private string apiKey;
        private string apiSecret;
        private int sessionId;
        private string sessionSecret;

        /// <summary>
        /// Constructor which get api key and secret
        /// </summary>
        /// <param name="newApiKey">Api key</param>
        /// <param name="newApiSecret">Api secret</param>
        /// <param name="newSessionId">Session user id</param>
        /// <param name="newSessionSecret">Session secret key</param>
        public TicketApi(string newApiKey, string newApiSecret, int newSessionId = 0, string newSessionSecret = null)
        {
            apiKey = newApiKey;
            apiSecret = newApiSecret;
            sessionId = newSessionId;
            sessionSecret = newSessionSecret;
        }

        /// <summary>
        /// Get all tickets, from one user
        /// </summary>
        /// <returns>List with Ticket objects</returns>
        public List<Ticket> GetTickets(int userId)
        {
            RestRequest request = new RestRequest("tickets/{id}", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse<List<Ticket>> response = client.Execute<List<Ticket>>(request);

            AnalysResponse(response.StatusCode, "Get", "Tickets", "from user id " + userId);


            return response.Data;
        }

        /// <summary>
        /// Get a ticket by ID from the system Returns a single ticket
        /// </summary>
        /// <param name="ticketId">ID of the ticket</param>
        /// <returns>Ticket object</returns>
        public Ticket GetTicketById(int ticketId)
        {
            RestRequest request = new RestRequest("ticket/{id}", Method.GET);
            RestClient client = PrepareRequest(ref request);

            request.AddUrlSegment("id", ticketId);
            IRestResponse<Ticket> response = client.Execute<Ticket>(request);

            AnalysResponse(response.StatusCode, "Get", "Ticket", "with id " + ticketId);

            return response.Data;
        }

        /// <summary>
        /// Buy ticket
        /// </summary>
        /// <param name="booking">Booking object to send as Json</param>
        public void PostTicket(Booking booking)
        {
            RestRequest request = new RestRequest("Ticket/", Method.POST);
            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(booking);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response.StatusCode, "Buy", "new ticket");

            if(response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new FormatException(string.Format("A ticket are already booked with this seat ({0})", booking.Ticket.SeatNumber));
            }
            else if(response.StatusCode == HttpStatusCode.PaymentRequired)
            {
                throw new Exception(string.Format("Payment fail, recheck your payment details!"));
            }
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="login">Login object with login information</param>
        /// <returns>Session information object</returns>
        public SessionInfo PostLoginIn(Login login)
        {
            RestRequest request = new RestRequest("Ticket/", Method.POST);
            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(login);

            IRestResponse<SessionInfo> response = client.Execute<SessionInfo>(request);

            AnalysResponse(response.StatusCode, "Login", "user", login.Username);

            return response.Data;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest("Users/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<User>> response = client.Execute<List<User>>(request);

            AnalysResponse(response.StatusCode, "Get", "users");

            return response.Data;
        }

        /// <summary>
        /// Get user with id
        /// </summary>
        /// <param name="userId">User index</param>
        /// <returns>User object</returns>
        public User GetUser(int userId)
        {
            RestRequest request = new RestRequest("Users/{id}", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse<User> response = client.Execute<User>(request);

            AnalysResponse(response.StatusCode, "Get", "user", "with id" + userId);

            return response.Data;
        }

        /// <summary>
        /// Change user
        /// </summary>
        /// <param name="userId">User index</param>
        public void PutUser(int userId)
        {
            RestRequest request = new RestRequest("Users/{id}", Method.PUT);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response.StatusCode, "Edit", "user", "with id" + userId);
        }

        /// <summary>
        /// Request to remove user
        /// </summary>
        /// <param name="userId">User index</param>
        public void DeleteUser(int userId)
        {
            RestRequest request = new RestRequest("Users/{id}", Method.DELETE);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response.StatusCode, "Delete", "user", "with id" + userId);
        }

        /// <summary>
        /// Request to add new user
        /// </summary>
        /// <returns>Add user object</returns>
        public User PostUser()
        {
            RestRequest request = new RestRequest("Users/", Method.POST);

            RestClient client = PrepareRequest(ref request);
            IRestResponse<User> response = client.Execute<User>(request);

            AnalysResponse(response.StatusCode, "add", "new user");

            return response.Data;
        }

        /// <summary>
        /// Prepare RestRequest and RestClient
        /// </summary>
        /// <param name="request">RestRequest object to prepare</param>
        /// <returns>RestClient</returns>
        private RestClient PrepareRequest(ref RestRequest request)
        {
            RestClient client = new RestClient("https://rmbl-flightticketapi.azurewebsites.net");

            string timestamp = DateTime.Now.ToString("r");
            string hashApiSecretTimestamp = HashMessageByKey(apiSecret, timestamp);

            request.AddHeader("Authentication", apiKey + ":" + hashApiSecretTimestamp);
            request.AddHeader("Timestamp", timestamp);

            if (sessionId != 0)
            {
                string hashSessionSecretTimestamp = HashMessageByKey(sessionSecret, timestamp);

                request.AddHeader("User-Authentication", sessionId + ":" + hashSessionSecretTimestamp);
            }

            return client;
        }

        /// <summary>
        /// Analys response status code
        /// </summary>
        /// <param name="statusCode">Response status code</param>
        /// <param name="toDo">What did request ask todo</param>
        /// <param name="toWhat">What did request ask to do that to</param>
        /// <param name="withId">Did request ask to do that to a specfic index? Then specify that to</param>
        private void AnalysResponse(HttpStatusCode statusCode, string toDo, string toWhat, string withId = null)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(string.Format("Unauthorized {0} {1} {2}", toDo, toWhat, withId));

                case HttpStatusCode.NotFound:
                    throw new NotSupportedException(string.Format("Not found api command for {0} {1} {2}", toDo, toWhat, withId));

                case HttpStatusCode.BadRequest:
                    throw new FormatException(string.Format("Sended values to {0} {1} {2}, are not well formated", toDo, toWhat, withId));

                case HttpStatusCode.NoContent:
                    throw new NullReferenceException(string.Format("{0} {1} is not found", toWhat, withId));
            }
        }

        private string HashMessageByKey(string apiSecret, string message)
        {
            var key = Encoding.UTF8.GetBytes(apiSecret.ToUpper());
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }
    }
}
