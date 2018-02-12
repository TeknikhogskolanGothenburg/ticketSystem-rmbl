using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TicketSystem.RestApiClient.Model;

namespace TicketSystem.RestApiClient
{
    public class TicketApi : ITicketApi
    {
        // Implemented using RestSharp: http://restsharp.org/
            
        public List<Ticket> TicketGet()
        {
            var client = new RestClient("https://rmbl.database.windows.net");
            client.Authenticator = new HttpBasicAuthenticator("rmblA", "QAwsedrf123@@");

            var request = new RestRequest("Ticket", Method.GET);
            var response = client.Execute<List<Ticket>>(request);
            return response.Data;
        }

        public Ticket TicketTicketIdGet(int ticketId)
        {
            var client = new RestClient("https://rmbl.database.windows.net");
            var request = new RestRequest("ticket/{id}", Method.GET);
            request.AddUrlSegment("id", ticketId);
            var response = client.Execute<Ticket>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Ticket with ID: {0} is not found", ticketId));
            }

            return response.Data;
        }
    }
}
