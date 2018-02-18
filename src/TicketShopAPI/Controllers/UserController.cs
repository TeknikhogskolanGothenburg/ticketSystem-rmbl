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
using TicketShopAPI.APISecurity;
using AuthenticationLibrary;

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

        /// <returns> all registered customers as json | StatusCode: 200 OK</returns>
        /// <returns> no users registered | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 401 Unauthorized</returns>
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                List<User> allusers = new List<User>();
                allusers = TicketDb.UserFindAll();
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
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new string[] { "access denied" };
            }
        }

        /// <summary>
        /// querries database for users matching a condition
        /// </summary>
        /// <param name="id">user id used in database querry</param>
        /// <QueryString Value="grade">searches for user group, 1: customers, 2: administrators 3: Sensei</param>
        /// <returns> all matching customers as json | StatusCode: 200 Ok</returns>
        /// <returns> no such user registered | StatusCode: 204 NoContent</returns>
        /// <returns> access denied | StatusCode: 407 Unauthorized</returns>
        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(int id)
        {            
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                User user = new User();
                user = TicketDb.UserFind(id);
                if (user != null)
                {                    
                    return JsonConvert.SerializeObject(user);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return "no such user registered";
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return "access denied";
            }            
        }

        /// <summary>
        /// querries database for all ticked onwne by a particular user
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // POST: api/5/Ticket
        [HttpGet("{id}/Ticket")]
        public IEnumerable<string> GetUserTicket(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                List<Ticket> tickets = TicketDb.TicketforUserFind(id);
                return tickets.Select(t => JsonConvert.SerializeObject(t));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new string[] { "access denied" };
            }
        }

        /// <summary>
        /// login user by creating new session
        /// </summary>
        /// <param name="data">username and password info</param>
        /// <returns>an object repesenting the new session + user info | StatusCode: 200 Ok</returns>
        /// <returns>null | StatusCode: 407 Unauthorized</returns>
        // POST: api/5/Ticket
        [HttpPost("/Login")]
        public LoginAnswer PostLogin([FromBody]JObject data)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = -1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                Login loginInfo = data["Login"].ToObject<Login>();
                User user = TicketDb.UserFind(loginInfo.Username);
                if(SecurePasswordHasher.Verify(loginInfo.Password, user.Password))
                {
                    string secret = Guid.NewGuid().ToString();
                    Session session = TicketDb.SessionAdd(user.Id, secret, DateTime.Now);
                    return new LoginAnswer {
                        SessionId = session.ID,
                        SessionSecret = session.Secret,
                        UserGrade = user.Grade,
                        UserId = user.Id,
                        Username = user.Username }; 
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return null;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return null;
            }
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user">new user to be added to database</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // POST: api/User
        [HttpPost]
        public void Post([FromBody]JObject data)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 0;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                User user = data["User"].ToObject<User>();

                if (user == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                user.Password = SecurePasswordHasher.Hash(user.Password);
                
                try
                {
                    TicketDb.UserAdd(user.Username, user.Password, user.Email, user.FirstName, user.LastName, user.City, user.ZipCode, user.Address, user.Grade);
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

        /// <summary>
        /// updates a user based on id
        /// </summary>
        /// <param name="data">user data used to update</param>
        /// <param name="id">id of user to be updated</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 404 NotFound</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        /// NOTE: Make sure 'user' class has complete property values, only password and DeletedUser can be skipped
        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]JObject data)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 1;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                Response.Headers.Add("Authorization", Authentication.AuthenticationHeader(security.ApiKey,security.ApiSecret,security.Timestamp));
                User user;
                if (!(security.User.Grade > 1) && security.UserId != id)
                {
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

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
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }

        /// <summary>
        /// deletes a user based on id
        /// </summary>
        /// <param name="id">id of user to be deleted</param>
        /// <returns>void | StatusCode: 200 Ok</returns>
        /// <returns>void | StatusCode: 400 BadRequest</returns>
        /// <returns>void | StatusCode: 407 Unauthorized</returns>
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string apiKeyData = Request.Headers["Authorization"];
            string sessionData = Request.Headers["User-Authentication"];
            string timeStamp = Request.Headers["Timestamp"];
            int gradeRestriction = 2;
            if (security.IsAuthorised(timeStamp, apiKeyData, sessionData, gradeRestriction))
            {
                bool deleteSuccessful = TicketDb.UserDelete(id);
                if (!deleteSuccessful)
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
