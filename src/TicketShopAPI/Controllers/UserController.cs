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
using Newtonsoft.Json.Linq;


namespace TicketShopAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private Security security = new Security();
        private TicketDatabase TicketDb = new TicketDatabase();

        /// <summary>
        /// querries database for all users
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <returns> all registered customers as json</returns>
        /// <returns> no users registered | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<User> allusers = new List<User>();
            if (security.IsAuthorised("NotSureyet"))
            {
                allusers = TicketDb.UserFind("");
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

        /// <summary>
        /// querries database for users matching a condition
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="id">user id used in database querry</param>
        /// <QueryString Value="grade">searches for user group, 1: customers, 2: administrators 3: Sensei</param>
        /// <returns> all matching customers as json | StatusCode: 200 Ok</returns>
        /// <returns> no such user registered | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // GET: api/User/5
        [HttpGet("{id}")]
        public IEnumerable<string> Get(int id)
        {
            List<User> users = new List<User>();
            if (security.IsAuthorised("NotSureYet"))
            {
                users = TicketDb.UserFind(id.ToString());
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

        [HttpGet("{id}/Ticket")]
        public IEnumerable<string> GetUserTicket(int id)
        {
            return new string[] { "", "" };
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="user">new user to be added to database</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
            // POST: api/User
        [HttpPost]
        public void Post([FromBody]JObject data)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                User user = data["User"].ToObject<User>();
                if (user == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                //string newSalt = security.GenerateSalt();
                //string encryptedPassword = security.GenerateSHA256Hash(user.Password, newSalt);

                string encryptedPassword = "placeholder";

                try
                {
                    TicketDb.UserAdd(user.Username, encryptedPassword, user.Email, user.FirstName, user.LastName, user.City, user.ZipCode, user.Address, user.Grade);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
            }

        }

        /// <summary>
        /// updates a user based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="data">user data used to update</param>
        /// <param name="id">id of user to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // NOTE: Make sure 'user' class has complete property values, only password can be skipped by sending null
        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]JObject data)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                User user;
                try
                {
                    user = data["User"].ToObject<User>();
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                // --- new passord code here ---
                //string newSalt = security.GenerateSalt();
                //string encryptedPassword = security.GenerateSHA256Hash(user.Password, newSalt);

                string encryptedPassword = "temporary placeholder";

                if (user.Password == null)
                {
                    encryptedPassword = null;
                }
                try
                {
                    TicketDb.UserModify(id, user.Username, encryptedPassword, user.Email, user.FirstName, user.LastName, user.City, user.ZipCode, user.Address, user.Grade);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
            }
        }

        /// <summary>
        /// deletes a user based on id
        /// </summary>
        /// <param name="NotSureYet">value that determines if client has access to the api</param>
        /// <param name="id">id of user to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 ProxyAuthenticationRequired</returns>
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (security.IsAuthorised("NotSureYet"))
            {
                bool deleteSuccessful = TicketDb.UserDelete(id);
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
