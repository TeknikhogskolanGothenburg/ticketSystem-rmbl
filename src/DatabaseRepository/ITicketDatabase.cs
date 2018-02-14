using TicketSystem.DatabaseRepository.Model;
using System.Collections.Generic;

namespace TicketSystem.DatabaseRepository
{
    public interface ITicketDatabase
    {
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
    }
}
