using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.RestApiClient;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackOffice.Controllers
{
    public class UsersController : Controller
    {
        private TicketApi ticketApi;
        private ApiInformation api;
        private MessagesHandler messagesHandler;

        /// <summary>
        /// Default constructor, prepare api
        /// </summary>
        public UsersController()
        {
            api = new ApiInformation();
        }

        /// <summary>
        /// Before actions are executing, do this... 
        /// </summary>
        /// <param name="context">Action context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (TempData["Userid"] != null)
            {
                ticketApi = new TicketApi(api.Key, api.Secret, (int)TempData["SessionId"], (string)TempData["SessionSecret"]);
            }
            else
            {
                ticketApi = new TicketApi(api.Key, api.Secret);
            }

            messagesHandler = new MessagesHandler(TempData);

            // Execute action
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// List of users
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            try
            {
                List<User> users = ticketApi.GetUsers();

                return View(users);
            }
            catch(Exception ex)
            {
                messagesHandler.Add("danger", ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Edit(string id)
        {
            int userId;
            if ((String.IsNullOrEmpty(id)) || !int.TryParse(id, out userId))
            {
                messagesHandler.Add("danger", "No user find, to edit");
            }
            else
            {
                try
                {
                    User user = ticketApi.GetUser(userId);
                    return View(user);
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                }
            }


            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edit user, with post
        /// </summary>
        /// <param name="id">User index</param>
        /// <param name="newUser">User object with post information</param>
        /// <returns>View or redirect</returns>
        [HttpPost("{id}")]
        public IActionResult Edit(string id, User user)
        {
            int userId;
            if ((String.IsNullOrEmpty(id)) || !int.TryParse(id, out userId))
            {
                messagesHandler.Add("danger", "No user find, to edit");

                return RedirectToAction("Index");
            }

            if (user == null)
            {
                return View(new User());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticketApi.PutUser(userId, user);
                    messagesHandler.Add("success", "User are successfully edit!");

                    return RedirectToAction("Edit", new { id = userId });
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                }
            }

            return View(user);
        }

        /// <summary>
        /// Remove user, without post
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Remove(string id)
        {
            int userId;
            if ((String.IsNullOrEmpty(id)) || !int.TryParse(id, out userId))
            {
                messagesHandler.Add("danger", "No user find, to remove");
            }
            else
            {
                try
                {
                    User user = ticketApi.GetUser(userId);

                    return View(user);
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Remove user, with post
        /// </summary>
        /// <param name="id">User index</param>
        /// <param name="remove">Bool-value as string</param>
        /// <returns>Redirect or view</returns>
        [HttpPost("{id}")]
        public IActionResult Remove(string id, string remove = "false")
        {
            int userId;
            if ((String.IsNullOrEmpty(id)) || !int.TryParse(id, out userId))
            {
                messagesHandler.Add("danger", "No user find, to remove");
            }
            else
            {
                if (remove == "true")
                {
                    try
                    {
                        ticketApi.DeleteUser(userId);
                        messagesHandler.Add("success", "User was successfully removed!");

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        messagesHandler.Add("danger", ex.Message);
                    }
                }

                if (remove != "false")
                {
                    try
                    {
                        User user = ticketApi.GetUser(userId);

                        if (remove != "true")
                        {
                            messagesHandler.Add("warning", "Something went wrong!");
                        }

                        return View(user);
                    }
                    catch(Exception ex)
                    {
                        messagesHandler.Add("danger", ex.Message);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Get a list of buyed tickets for a user
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        [HttpGet("{id}")]
        public IActionResult Tickets(string id)
        {
            int userId;
            if ((String.IsNullOrEmpty(id)) || !int.TryParse(id, out userId))
            {
                messagesHandler.Add("danger", "No user find, to get tickets for");
            }
            else
            {
                try
                {
                    List<Ticket> tickets = ticketApi.GetTicketsByUser(userId);
                    return View(tickets);
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                }
            }

            return RedirectToAction("Index");
        }
    }
}