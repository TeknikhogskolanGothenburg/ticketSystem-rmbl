using TicketSystem.DatabaseRepository.Model;
using System.Collections.Generic;

namespace TicketSystem.DatabaseRepository
{
    public interface ITicketDatabase
    {3
        /// <summary>
        /// Add a new User to the database
        /// </summary>
        /// <param name="firstName">First name of the User</param>
        /// <param name="LastName">Last name of the User</param>
        /// <param name="password">SAH256 hashed password of the user</param>
        /// <param name="salt">password salt, should this be here?</param>
        /// <param name="city">Users city of residence</param>
        /// <param name="address">Users address</param>
        /// <param name="grade">Access level of the user {1: Normal customer, 2: Admin, 3: Sensei}</param>
        /// <returns>An object representing the newly created User</returns>
        User UserAdd(string firstName, string LastName, string password, string salt, string city, string address, int grade);

        /// <summary>
        /// locates one, all or subcategory of users
        /// </summary>
        /// <param name="query">value that db querry tries to match</param>
        /// <returns>An object List representing the users matching the search querry</returns>
        List<User> UserFind(string query);

        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <param name="firstName">user property to be written, never ignored</param>
        /// <param name="lastName">user property to be written, never ignored</param>
        /// <param name="password">user property to be written, null allowed - ignored</param>
        /// <param name="salt">user property to be written, ignored if password is null</param>
        /// <param name="city">user property to be written, never ignored</param>
        /// <param name="address">user property to be written, never ignored</param>
        /// <param name="grade">user property to be written, never ignored</param>
        /// <param name="id">value that db querry tries to match, never ignored</param>
        /// <returns>void</returns>
        User UserModify(string firstName, string lastName, string password, string salt, string city, string address, int grade, string id);

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="id">value that db querry tries to match for removal</param>
        /// <returns>a bool indicating whether the delete command was successful</returns>
        bool UserDelete(string id);

        /// <summary>
        /// Find all venus matching the query
        /// </summary>
        /// <param name="query">A text which is user i looking for in the venues</param>
        /// <returns>A list of venus matching the query</returns>
        //List<Venue> VenuesFind(string query);
    }
}
