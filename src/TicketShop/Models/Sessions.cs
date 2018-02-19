using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketShop.Models
{
    public class Sessions
    {
        private HttpRequest request;
        private HttpResponse response;
        private Dictionary<string, Dictionary<string, Session>> sessions;
        private string thisRunCookieKey;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Sessions()
        {
            sessions = new Dictionary<string, Dictionary<string, Session>> ();
        }

        /// <summary>
        /// Initialize sessions with Http context
        /// </summary>
        /// <param name="context">Http context</param>
        public void Intialize(HttpContext context)
        {
            response = context.Response;
            request = context.Request;

            thisRunCookieKey = null;
            SetSessionsCookieKey();

            EmptyExpiredSessions();
        }

        /// <summary>
        /// Add session value
        /// </summary>
        /// <param name="key">Session key</param>
        /// <param name="value">Session value</param>
        /// <param name="expires">When should session expire? (In millisecound) [Default: 2 hours]</param>
        public void Add(string key, object value, int expires = 60 * 60 * 1000 * 2)
        {
            DateTime expireDate = DateTime.Now.AddMilliseconds(expires);

            Dictionary<string, Session> visitorSessions = GetVisitorSessions();

            if (visitorSessions.ContainsKey(key))
            {
                visitorSessions[key] = new Session(value, expires);
            }
            else
            {
                visitorSessions.Add(key, new Session(value, expires));
            }
        }

        /// <summary>
        /// Check if session exist
        /// </summary>
        /// <returns>Validate result</returns>
        public bool Exist(string key)
        {
            Dictionary<string, Session> visitorSessions = GetVisitorSessions();

            if (visitorSessions.ContainsKey(key))
            {
                return (visitorSessions[key] != null);
            }

            return false;
        }


        /// <summary>
        /// Get session value with key
        /// </summary>
        /// <param name="key">Session key</param>
        /// <returns>Session value</returns>
        public object Get(string key)
        {
            if(request.Cookies["session"] != null)
            {
                Dictionary<string, Session> visitorSessions = GetVisitorSessions();

                if (visitorSessions.ContainsKey(key))
                {
                    try
                    {
                        Session session = visitorSessions[key];
                        return session.Value;
                    }
                    catch
                    {
                        Remove(key);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Remove session with key
        /// </summary>
        /// <param name="key">Session key</param>
        public void Remove(string key)
        {
            if (request.Cookies["session"] != null)
            {
                Dictionary<string, Session> visitorSessions = GetVisitorSessions();

                if (visitorSessions.ContainsKey(key))
                {
                    visitorSessions[key].Value = null;
                    visitorSessions.Remove(key);
                }
            }
        }

        /// <summary>
        /// Remove all sessions
        /// </summary>
        public void RemoveAll()
        {
            sessions = new Dictionary<string, Dictionary<string, Session>>();
        }

        /// <summary>
        /// Remove all expired sessions
        /// </summary>
        public void EmptyExpiredSessions()
        {
            foreach (Dictionary<string, Session> value in sessions.Values)
            {
                foreach (KeyValuePair<string, Session> pair in value)
                {
                    if (pair.Value.Expired)
                    {
                        sessions.Remove(pair.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Get all Sessions for a cookie key (aka one visitor/browser)
        /// </summary>
        /// <returns>Dictionary with all sessions for a cookie key</returns>
        private Dictionary<string, Session> GetVisitorSessions()
        {
            if (request.Cookies["session"] == null || !String.IsNullOrEmpty(thisRunCookieKey) || !sessions.ContainsKey(thisRunCookieKey))
            {
                SetSessionsCookieKey();
            }

            return sessions[thisRunCookieKey];
        }

        /// <summary>
        /// Set visitor's sessions cookie key
        /// </summary>
        private void SetSessionsCookieKey()
        {
            if (request.Cookies["session"] != null)
            {
                thisRunCookieKey = request.Cookies["session"];
            }
            else
            {
                if (String.IsNullOrEmpty(thisRunCookieKey))
                {
                    // Make a cookie key and check so it's unique, otherwish make a new and check...
                    do
                    {
                        thisRunCookieKey = Guid.NewGuid().ToString();
                    } while (sessions.ContainsKey(thisRunCookieKey));
                }

                response.Cookies.Append("session", thisRunCookieKey);
            }

            if (!sessions.ContainsKey(thisRunCookieKey))
            {
                sessions.Add(thisRunCookieKey, new Dictionary<string, Session>());
            }
        }
    }
}
