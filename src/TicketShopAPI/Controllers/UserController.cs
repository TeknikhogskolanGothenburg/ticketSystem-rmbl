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

namespace TicketShopAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private Security security = new Security();
        public UserController()
        {
            // --- authorisation code ---
            //string raw = Request.Headers["Authorization"];
            //string[] ApiData =  raw.Split(':');

            //string ApiKey = ApiData[0];
            //string ApiSecret = ApiData[1];

            //TicketDatabase ticketDb = new TicketDatabase();
            //ticketDb.ApiKeyFindFind();
        }



        // GET: api/User
        /// <summary>
        /// querries database for all users
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> all registered customers as json</returns>
        /// <returns> no users registered | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 ProxyAuthenticationRequired</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<User> allusers = new List<User>();
            if (security.IsAuthorised("NotSureyet"))
            {
                TicketDatabase ticketDb = new TicketDatabase();
                allusers = ticketDb.UserFind("");
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
                return new string[] { "access denied" };
            }
            if (allusers.Count != 0)
            {

                return allusers.Select(u => JsonConvert.SerializeObject(u));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NoContent;
                return new string[] { "no users registered" };
            }
        }

        // GET: api/User/5
        /// <summary>
        /// querries database for users matching a condition
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="id">user id used in database querry</param>
        /// <QueryString Value="grade">searches for user group, 1: customers, 2: administrators 3: Sensei</param>
        /// <returns> all matching customers as json | StatusCode: 200 Ok</returns>
        /// <returns> no such user registered | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 ProxyAuthenticationRequired</returns>
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<string> Get(int id)
        {
            List<User> users = new List<User>();
            if (security.IsAuthorised("NotSureYet"))
            {
                TicketDatabase ticketDb = new TicketDatabase();
                if (Request.Query.ContainsKey("grade"))
                {
                    users = ticketDb.UserGroupFind(id.ToString(), Request.Query["grade"]);
                }
                else
                {
                    users = ticketDb.UserFind(id.ToString());
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
                return new string[] { "access denied" };
            }
            if (users.Count != 0)
            {
                return users.Select(u => JsonConvert.SerializeObject(u));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NoContent;
                return new string[] { "no such user registered" };
            }
        }

        // POST: api/User
        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="user">new user to be added to database</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        [HttpPost]
        public void Post([FromBody]User user)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                if (user == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                //string newSalt = security.GenerateSalt();
                //string encryptedPassword = security.GenerateSHA256Hash(user.Password, newSalt);

                string encryptedPassword = "placeholder";

                TicketDatabase ticketDb = new TicketDatabase();
                ticketDb.UserAdd(user.Username, encryptedPassword, user.Email, user.FirstName, user.LastName, user.City, user.ZipCode, user.Address, user.Grade);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
            }

        }

        // PUT: api/User/5
        /// <summary>
        /// updates a user based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="user">user data used to update</param>
        /// <param name="id">id of user to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // NOTE: Make sure 'user' class has complete parameter values, only password can be skipped by sending null
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]User user)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                if (user == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                // --- new passord code here ---
                //string newSalt = security.GenerateSalt();
                //string encryptedPassword = security.GenerateSHA256Hash(user.Password, newSalt);

                TicketDatabase ticketDb = new TicketDatabase();
                string encryptedPassword = "temporary placeholder";

                if (user.Password == null)
                {
                    encryptedPassword = null;
                }
                User updatedUser = ticketDb.UserModify(id, user.Username, encryptedPassword, user.Email, user.FirstName, user.LastName, user.City, user.ZipCode, user.Address, user.Grade);
                if (updatedUser == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
            }
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// deletes a user based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="id">id of user to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                TicketDatabase ticketDb = new TicketDatabase();
                bool deleteSuccessful = ticketDb.UserDelete(id.ToString());
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
