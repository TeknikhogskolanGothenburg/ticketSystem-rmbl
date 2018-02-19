using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TicketSystem.RestApiClient.Model;
using AuthenticationLibrary;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace TicketSystem.RestApiClient
{
    public class TicketApi : ITicketApi
    {
        private string apiKey;
        private string apiSecret;
        private int sessionId;
        private string sessionSecret;

        // Implemented using RestSharp: http://restsharp.org/

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

        public List<AirPort> GetAirPorts()
        {
            RestRequest request = new RestRequest("api/AirPort/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
            List<AirPort> airPorts = new List<AirPort>();
            response.Data.ForEach(a => airPorts.Add(JsonConvert.DeserializeObject<AirPort>(a)));

            AnalysResponse(response, "Get", "AirPort");

            return airPorts;
        }

        public List<Flight> GetFlights()
        {
            RestRequest request = new RestRequest("api/Flight/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
            List<Flight> flights = new List<Flight>();
            response.Data.ForEach(a => flights.Add(JsonConvert.DeserializeObject<Flight>(a)));

            AnalysResponse(response, "Get", "Flights");

            return flights;
        }

        /// <summary>
        /// Add flight
        /// </summary>
        /// <param name="flight">Flight object to send as Json</param>
        public int PostFlight(Flight flight)
        {
            RestRequest request = new RestRequest("api/Flight/", Method.POST);
            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(flight);

            IRestResponse response = client.Execute(request);

            List<Parameter> headers = AnalysResponse(response, "Add", "new flight");

            Parameter RedirectId = headers.Find(h => h.Name == "RedirectId");

            int id;

            if (int.TryParse(RedirectId.Value.ToString(), out id))
            {
                return id;
            }

            throw new Exception("Api response incorrect, report to site ");
        }

        /// <summary>
        /// Get all tickets, from one user
        /// </summary>
        /// <returns>List with Ticket objects</returns>
        public List<Ticket> GetTicketsByUser(int userId)
        {
            RestRequest request = new RestRequest("api/User/{id}/ticket", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse<List<Ticket>> response = client.Execute<List<Ticket>>(request);

            AnalysResponse(response, "Get", "Tickets", "from user id " + userId);

            return response.Data;
        }

        /// <summary>
        /// Get a ticket by ID from the system Returns a single ticket
        /// </summary>
        /// <param name="ticketId">ID of the ticket</param>
        /// <returns>Ticket object</returns>
        public Ticket GetTicketById(int ticketId)
        {
            RestRequest request = new RestRequest("api/Ticket/{id}", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", ticketId);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());
            IRestResponse response = client.Execute(request);

            AnalysResponse(response, "Get", "Ticket", "with id " + ticketId);

            //return JsonConvert.DeserializeObject<Ticket>(response.Content);

            return new Ticket();

        }

        /// <summary>
        /// Buy ticket
        /// </summary>
        /// <param name="booking">Booking object to send as Json</param>
        public void PostTicket(Booking booking)
        {
            RestRequest request = new RestRequest("api/Ticket/", Method.POST);
            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(booking);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response, "Buy", "new ticket");

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
            RestRequest request = new RestRequest("api/User/Login", Method.POST);
            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(login);

            IRestResponse<SessionInfo> response = client.Execute<SessionInfo>(request);

            AnalysResponse(response, "Login", "user", login.Username);

            return response.Data;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest("api/User/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<User>> response = client.Execute<List<User>>(request);

            AnalysResponse(response, "Get", "users");

            return response.Data;
        }

        /// <summary>
        /// Get user with id
        /// </summary>
        /// <param name="userId">User index</param>
        /// <returns>User object</returns>
        public User GetUser(int userId)
        {
            RestRequest request = new RestRequest("api/User/{id}", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            /*IRestResponse<User> response = client.Execute<User>(request);

            AnalysResponse(response, "Get", "user", "with id" + userId);*/


            IRestResponse<string> response = (IRestResponse<string>)client.Execute(request);
            List<User> user = new List<User>();
            //response.Data.ForEach(a => user.Add(JsonConvert.DeserializeObject<User>(a)));

            AnalysResponse(response, "Get", "User");


            //return airPorts;

            return new User();
        }

        /// <summary>
        /// Change user
        /// </summary>
        /// <param name="userId">User index</param>
        /// <param name="user">User object to replace old one</param>
        public void PutUser(int userId, User user)
        {
            RestRequest request = new RestRequest("api/User/{id}", Method.PUT);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response, "Edit", "user", "with id" + userId);
        }

        /// <summary>
        /// Request to remove user
        /// </summary>
        /// <param name="userId">User index</param>
        public void DeleteUser(int userId)
        {
            RestRequest request = new RestRequest("api/User/{id}", Method.DELETE);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", userId);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response, "Delete", "user", "with id" + userId);
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

            AnalysResponse(response, "add", "new user");

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
            request.AddHeader("Timestamp", timestamp);
            request.AddHeader("Authentication", Authentication.AuthenticationHeader(apiKey, apiSecret, timestamp));

            if (sessionId != 0)
            {
                request.AddHeader("User-Authentication", Authentication.UserAuthenticationHeader(sessionId, sessionSecret, timestamp));
            }

            return client;
        }

        /// <summary>
        /// Analys response status code
        /// </summary>
        /// <param name="response">Response status code</param>
        /// <param name="toDo">What did request ask todo</param>
        /// <param name="toWhat">What did request ask to do that to</param>
        /// <param name="withId">Did request ask to do that to a specfic index? Then specify that to</param>
        private List<Parameter> AnalysResponse(IRestResponse response, string toDo, string toWhat, string withId = null)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(string.Format("Unauthorized {0} {1} {2}", toDo, toWhat, withId));

                case HttpStatusCode.InternalServerError:
                    throw new Exception(String.Format("Api server did got internal server error on request todo {0} {1} {2}"));

                case HttpStatusCode.NotFound:
                    throw new NotSupportedException(string.Format("Not found api command for {0} {1} {2}", toDo, toWhat, withId));

                case HttpStatusCode.BadRequest:
                    throw new FormatException(string.Format("Sended values to {0} {1} {2}, are not well formated", toDo, toWhat, withId));

                case HttpStatusCode.NoContent:
                    throw new NullReferenceException(string.Format("{0} {1} is not found", toWhat, withId));
            }

            List<Parameter> headers = new List<Parameter>(response.Headers);



            return headers;
        }
    }
}
