using TicketSystem.DatabaseRepository.Model;
using System.Collections.Generic;
using System;

namespace TicketSystem.DatabaseRepository
{
    public interface ITicketDatabase
    {
        /// <summary>
        /// find one user
        /// </summary>
        /// <param name="id">user index</param>
        /// <returns>A user object</returns>
        User UserFind(int id);

        /// <summary>
        /// finds all users
        /// </summary>
        /// <returns>An object List representing the users</returns>
        List<User> UserFindAll();

        /// <summary>
        /// locates one, all or subcategory of users
        /// </summary>
        /// <param name="query">value that db querry tries to match</param>
        /// <returns>An object List representing the users matching the search querry</returns>
        List<User> UserFind(string query);

        /// <summary>
        /// Add a new User to the database
        /// </summary>
        /// <param name="firstName">First name of the User</param>
        /// <param name="LastName">Last name of the User</param>
        /// <param name="password">password of the user</param>
        /// <param name="city">Users city of residence</param>
        /// <param name="address">Users address</param>
        /// <param name="grade">Access level of the user {1: Normal customer, 2: Admin, 3: Sensei}</param>
        /// <returns>An object representing the newly created User</returns>
        User UserAdd(string username, string password, string email, string firstName, string lastName, string city, string zipCode, string address, int grade);

        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <param name="id">user property to be matched, required</param>
        /// <param name="username">user property to be updated, required</param>
        /// <param name="password">user property to be updated, required</param>
        /// <param name="email">user property to be updated, not required</param>
        /// <param name="firstName">user property to be updated, required</param>
        /// <param name="lastName">user property to be updated, required</param>
        /// <param name="city">user property to be updated, required</param>
        /// <param name="zipCode">user property to be updated, required</param>
        /// <param name="address">user property to be updated, required</param>
        /// <param name="grade">user property to be updated, requiredd</param>
        /// <returns>User object representing the newly updated user, password is always null</returns>
        User UserModify(int id, string username, string password, string email, string firstName, string lastName, string city, string zipCode, string address, int grade);

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="id">value that db querry tries to match for removal</param>
        /// <returns>a bool indicating whether the delete command was successful</returns>
        bool UserDelete(int id);

        /// <summary>
        /// Find a session
        /// </summary>
        /// <param name="id">Session index</param>
        /// <returns>A Session object representing row in table</returns>
        Session SessionFind(int id);

        /// <summary>
        /// Finds one or all Tickets
        /// </summary>
        /// <param name="query">value that db querry tries to match with ticket id ("" = all ticketrs)</param>
        /// <returns>A list of tickts found</returns>
        List<Ticket> TicketFind(string query);

        /// <summary>
        /// Finds a list of tickets belonging to a specific user
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>a list of tickets</returns>
        List<Ticket> TicketforUserFind(int id);

        /// <summary>
        /// adds a new ticket
        /// </summary>
        /// <param name="userId">ticket property</param>
        /// <param name="flightId">ticket property</param>
        /// <param name="seatNumber">ticket property</param>
        /// <param name="bookAt">ticket property</param>
        /// <returns>an object representing the newly added ticket</returns>
        Ticket TicketAdd(int userId, int flightId, int seatNumber, int bookAt);

        /// <summary>
        /// updates an excisting ticket
        /// </summary>
        /// <param name="id">id of ticket to be overwritten</param>
        /// <param name="userId">new ticket property to be written</param>
        /// <param name="flightId">new ticket property to be written</param>
        /// <param name="seatNumber">new ticket property to be written</param>
        /// <param name="bookAt">new ticket property to be written</param>
        /// <returns>an object representing the newly modified ticket</returns>
        Ticket TicketModify(int id, int userId, int flightId, int seatNumber, int bookAt);

        /// <summary>
        /// Deletes a ticket from the database
        /// </summary>
        /// <param name="id">id of ticket to be deleted</param>
        /// <returns>a bool indicating whether the delete command was successful</returns>
        bool TicketDelete(int id);

        /// <summary>
        /// Adds a new transaction to db
        /// </summary>
        /// <param name="paymentStatus">transaction propert</param>
        /// <param name="paymentReferenceId">transaction propert</param>
        /// <returns>an object representing the newly added transaction</returns>
        Transaction TransactionAdd(string paymentStatus, string paymentReferenceId);

        /// <summary>
        /// adds a  new TicketToTransaction to database
        /// </summary>
        /// <param name="ticketId">value that db querry tries to match for removal</param>
        /// <param name="transactionId">value that db querry tries to match for removal</param>
        /// <returns>a bool indicating success of the newly added TicketToTransaction</returns>
        bool TicketToTransactionAdd(int ticketId, int transactionId);

        /// <summary>
        /// Finds one or all flights
        /// </summary>
        /// <param name="query">value that db query tryes to match ("" = all flights)</param>
        /// <returns>a list of flights that where matched</returns>
        List<Flight> FlightFind(string query);

        /// <summary>
        /// adds a new flight to the db
        /// </summary>
        /// <param name="departureDate">flight property to be written</param>
        /// <param name="departurePor">flight property to be written</param>
        /// <param name="arrivalDate">flight property to be written</param>
        /// <param name="arrivalPort">flight property to be written</param>
        /// <param name="seats">flight property to be written</param>
        /// <returns>an object representing the newly added flight</returns>
        Flight FlightAdd(DateTime departureDate, int departurePort, DateTime arrivalDate, int arrivalPort, int seats);

        /// <summary>
        /// updates an excisting flight
        /// </summary>
        /// <param name="id">id of the flight that will be updated</param>
        /// <param name="departureDate">flight property to be overwritten</param>
        /// <param name="departurePort">flight property to be overwritten</param>
        /// <param name="arrivalDate">flight property to be overwritten</param>
        /// <param name="arrivalPort">flight property to be overwritten</param>
        /// <param name="seats">flight property to be overwritten</param>
        /// <returns>an object representing the updated flight</returns>
        Flight FlightModify(int id, DateTime departureDate, int departurePort, DateTime arrivalDate, int arrivalPort, int seats);

        /// <summary>
        /// Deletes a flight from the database
        /// </summary>
        /// <param name="id">value that db querry tries to match for removal</param>
        /// <returns>a bool indicating whether the delete command was successful</returns>
        bool FlightDelete(int id);

        /// <summary>
        /// finds one or all airports
        /// </summary>
        /// <param name="query">value that query tries to match ("" = all airports)</param>
        /// <returns>a list of Airports that matched the query</returns>
        List<AirPort> AirPortFind(string query);

        /// <summary>
        /// adds a new airport to the db
        /// </summary>
        /// <param name="name">air port property to be written</param>
        /// <param name="name">air port property to be written</param>
        /// <param name="name">air port property to be written</param>
        /// <returns>an object representing the newly added airport</returns>
        AirPort AirPortAdd(string name, string country, double utcOffset);

        /// <summary>
        /// updates an excisting airport
        /// </summary>
        /// <param name="id">the id of the airport that will be updated</param>
        /// <param name="name">property of airport that will be overwritten</param>
        /// <param name="id">property of airport that will be overwritten</param>
        /// <param name="id">property of airport that will be overwritten</param>
        /// <returns>an object representing the newly updated airport</returns>
        AirPort AirPortModify(int id, string name, string country, double utcOffset);

        /// <summary>
        /// Deletes an airport from the database
        /// </summary>
        /// <param name="id">the id of the airport that will be deleted</param>
        /// <returns>a bool indicating whether the delete command was successful</returns>
        bool AirPortDelete(int id);

        /// <summary>
        /// finds flights that departs from a specific airport
        /// </summary>
        /// <param name="portId">the id of the airport in question</param>
        /// <returns>a list of departure flights matching the given airport</returns>
        List<Flight> AirportDeparturesFind(int portId);

        /// <summary>
        /// finds flights that arrives at a specific airport
        /// </summary>
        /// <param name="id">value that db querry tries to match for removal</param>
        /// <returns>a list of arrival flights matching the given airport</returns>
        List<Flight> AirportArrivalsFind(int portId);

        /// <summary>
        /// finds all avaliable seats on a specific flight
        /// </summary>
        /// <param name="flightId">the id of the flight in question</param>
        /// <returns>a list of seat numbers</returns>
        List<int> AvaliableSeatsFind(int flightId);

        /// <summary>
        /// Finds an apiKey with a specific ID
        /// </summary>
        /// <param name="id">id used in query to match</param>
        /// <returns>the ApiKey string</returns>
        string APIKeyFind(int id);

        /// <summary>
        /// finds an api key secret
        /// </summary>
        /// <param name="id">the id of the api key sectet</param>
        /// <returns>the api key secret string</returns>
        string APISecretFind(int id);

        /// <summary>
        /// Finds an api key secret by api key key
        /// </summary>
        /// <param name="query">The api key key</param>
        /// <returns>the api key secret string</returns>
        string APISecretFind(string query);

        /// <summary>
        /// finds all transactions
        /// </summary>
        /// <returns>a list containing objects representing a transaction</returns>
        List<Transaction> TransactionFind();
    }
}
