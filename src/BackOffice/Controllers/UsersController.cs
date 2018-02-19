using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.RestApiClient;
using BackOffice.Models;
using TicketSystem.RestApiClient.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BackOffice.Controllers
{
    public class UsersController : Controller
    {
        private IConfigurationRoot config;
        private Sessions sessions;
        private TicketApi ticketApi;
        private MessagesHandler messagesHandler;
        private ILogger<UsersController> logger;

        /// <summary>
        /// Constructor sith logging, sessions and app settings
        /// </summary>
        /// <param name="newLogger">Logger</param>
        /// <param name="newConfig">App settings</param>
        /// <param name="newSessions">Sessions</param>
        public UsersController(ILogger<UsersController> newLogger, IConfigurationRoot newConfig, Sessions newSessions)
        {
            config = newConfig;
            sessions = newSessions;
            logger = newLogger;
        }

        /// <summary>
        /// Before actions are executing, do this... 
        /// </summary>
        /// <param name="context">Action context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sessions.Intialize(context.HttpContext);
            ViewData["Sessions"] = sessions;

            if (sessions.Get("UserId") != null)
            {
                ticketApi = new TicketApi(config["Api:Key"], config["Api:Secret"], (int)sessions.Get("SessionId"), (string)sessions.Get("SessionSecret"));
            }
            else
            {
                ticketApi = new TicketApi(config["Api:Key"], config["Api:Secret"]);
            }

            messagesHandler = new MessagesHandler(sessions);

            ViewData["Messages"] = messagesHandler;

            // Execute action
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// List of users
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            if (sessions.Exist("UserId"))
            {
                List<User> users;

                try
                {
                    users = ticketApi.GetUsers();
                }
                catch (Exception ex)
                {
                    messagesHandler.Add("danger", ex.Message);
                    users = new List<User>();
                }

                ViewBag.Users = users;

                return View();
            }

            messagesHandler.Add("warning", "You need to be login, to see all users");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        public IActionResult Edit(string id)
        {
            if (sessions.Exist("UserId"))
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

            messagesHandler.Add("warning", "You need to be login, to edit a user");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Edit user, with post
        /// </summary>
        /// <param name="id">User index</param>
        /// <param name="newUser">User object with post information</param>
        /// <returns>View or redirect</returns>
        [HttpPost]
        public IActionResult Edit(string id, User user)
        {
            if (sessions.Exist("UserId"))
            {
                int userId;
                if ((String.IsNullOrEmpty(id)) || !int.TryParse(id, out userId))
                {
                    messagesHandler.Add("danger", "No user find, to edit");

                    return RedirectToAction("Index");
                }

                if (user == null)
                {
                    try
                    {
                        user = ticketApi.GetUser(userId);
                        return View(user);
                    }
                    catch (Exception ex)
                    {
                        messagesHandler.Add("danger", ex.Message);
                    }
                }
                else if (ModelState.IsValid)
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

            messagesHandler.Add("warning", "You need to be login, to edit a user");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Remove user, without post
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        public IActionResult Remove(string id)
        {
            if (sessions.Exist("UserId"))
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

            messagesHandler.Add("warning", "You need to be login, to remove user");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Remove user, with post
        /// </summary>
        /// <param name="id">User index</param>
        /// <param name="remove">Bool-value as string</param>
        /// <returns>Redirect or view</returns>
        [HttpPost]
        public IActionResult Remove(string id, string remove)
        {
            if (sessions.Exist("UserId"))
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
                        catch (Exception ex)
                        {
                            messagesHandler.Add("danger", ex.Message);
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            messagesHandler.Add("warning", "You need to be login, to remove user");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Get a list of buyed tickets for a user
        /// </summary>
        /// <param name="id">User index</param>
        /// <returns>View</returns>
        public IActionResult Tickets(string id)
        {
            if (sessions.Exist("UserId"))
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
                        ViewBag.Tickets = ticketApi.GetTicketsByUser(userId);
                        return View();
                    }
                    catch (Exception ex)
                    {
                        messagesHandler.Add("danger", ex.Message);
                    }
                }

                return RedirectToAction("Index");
            }

            messagesHandler.Add("warning", "You need to be login, to see user's tickets");

            return RedirectToAction("Index", "Home");
        }
    }
}