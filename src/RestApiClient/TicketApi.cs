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

        /// <summary>
        /// Gets all Airports from api
        /// </summary>
        public List<AirPort> GetAirPorts()
        {
            RestRequest request = new RestRequest("api/AirPort/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
            
			AnalysResponse(response, "Get", "AirPort");
			
			List<AirPort> airPorts = new List<AirPort>();
            response.Data.ForEach(a => airPorts.Add(JsonConvert.DeserializeObject<AirPort>(a)));

            return airPorts;
        }

        public List<Flight> GetFlights()
        {
            RestRequest request = new RestRequest("api/Flight/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
			
			AnalysResponse(response, "Get", "Flights");
			
            List<Flight> flights = new List<Flight>();
            response.Data.ForEach(a => flights.Add(JsonConvert.DeserializeObject<Flight>(a)));

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

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
			
			AnalysResponse(response, "Get", "Tickets", "from user id " + userId);
			
			List<Ticket> tickets = new List<Ticket>();
            response.Data.ForEach(t => tickets.Add(JsonConvert.DeserializeObject<Ticket>(t)));

            return tickets;
        }

        public List<Flight> GetFlightsByAirportDate(int airport, string date)
        {
            RestRequest request = new RestRequest("api/AirPort/{id}/DepartureFlight/{date}", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", airport);
            request.AddUrlSegment("date", date);

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
			
			AnalysResponse(response, "Get", "Flights", "from airport id" + airport + " and date " + date);
			
            List<Flight> fligths = new List<Flight>();
            response.Data.ForEach(f => fligths.Add(JsonConvert.DeserializeObject<Flight>(f)));

            return fligths;
        }

        public List<int> GetFlightSeats(int flight)
        {
            RestRequest request = new RestRequest("api/Flight/{id}/AvaliableSeats", Method.GET);
            RestClient client = PrepareRequest(ref request);
            request.AddUrlSegment("id", flight);

            IRestResponse<List<int>> response = client.Execute<List<int>>(request);

            AnalysResponse(response, "Get", "Seats", "from flight id" + flight);

            List<int> seats = new List<int>();
            response.Data.ForEach(f => seats.Add(JsonConvert.DeserializeObject<int>(f.ToString())));

            return seats;
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

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);

            AnalysResponse(response, "Get", "Ticket", "with id " + ticketId);

            Ticket ticket;
            ticket = JsonConvert.DeserializeObject<Ticket>(response.Data[0]);

            return ticket;

        }

        /// <summary>
        /// Buy ticket
        /// </summary>
        /// <param name="booking">Booking object to send as Json</param>
        public int PostTicket(Booking booking)
        {
            RestRequest request = new RestRequest("api/Ticket/", Method.POST);
            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(booking);

            IRestResponse response = client.Execute(request);

            List<Parameter> headers = AnalysResponse(response, "Buy", "new ticket");
            
			AnalysResponse(response, "Buy", "new ticket");
			
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new FormatException(string.Format("A ticket are already booked with this seat ({0})", booking.Ticket.SeatNumber));
            }
            else if (response.StatusCode == HttpStatusCode.PaymentRequired)
            {
                throw new Exception(string.Format("Payment fail, recheck your payment details!"));
            }

            Parameter RedirectId = headers.Find(h => h.Name == "RedirectId");

            int id;

            if (int.TryParse(RedirectId.Value.ToString(), out id))
            {
                return id;
            }

            return 0;
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

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);

            AnalysResponse(response, "Login", "user", login.Username);

            SessionInfo sessionInfo = new SessionInfo();
            sessionInfo = JsonConvert.DeserializeObject<SessionInfo>(response.Data[0]);

            return sessionInfo;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest("api/User/", Method.GET);
            RestClient client = PrepareRequest(ref request);

            IRestResponse<List<string>> response = client.Execute<List<string>>(request);

            AnalysResponse(response, "Get", "users");

            List<User> users = new List<User>();
            response.Data.ForEach(a => users.Add(JsonConvert.DeserializeObject<User>(a)));

            return users;
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
            IRestResponse<List<string>> response = client.Execute<List<string>>(request);																   

            AnalysResponse(response, "Get", "User");

            User user = new User();
            user = JsonConvert.DeserializeObject<User>(response.Data[0]);

            return user;
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
            request.AddJsonBody(user);

            IRestResponse response = client.Execute(request);

            AnalysResponse(response, "Edit", "user", "with id " + userId);
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

            AnalysResponse(response, "Delete", "user", "with id " + userId);
        }

        /// <summary>
        /// Request to add new user
        /// </summary>
        /// <param name="user">User object to post</param>
        /// <returns>Add user object</returns>
        public User PostUser(User user)
        {
            RestRequest request = new RestRequest("Users/", Method.POST);

            RestClient client = PrepareRequest(ref request);
            request.AddJsonBody(user);

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
            RestClient client = new RestClient("http://localhost:58364/");

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
                    throw new Exception(String.Format("Api server did got internal server error on request todo {0} {1} {2}", toDo, toWhat, withId));

                case HttpStatusCode.NotFound:
                    throw new NotSupportedException(string.Format("Not found api command for {0} {1} {2}", toDo, toWhat, withId));

                case HttpStatusCode.UnsupportedMediaType:
                case HttpStatusCode.BadRequest:
                    throw new FormatException(string.Format("Sent values to {0} {1} {2}, are not well formated", toDo, toWhat, withId));

                case HttpStatusCode.NoContent:
                    throw new NullReferenceException(string.Format("{0} {1} is not found", toWhat, withId));
            }

            List<Parameter> headers = new List<Parameter>(response.Headers);



            return headers;
        }
    }
}