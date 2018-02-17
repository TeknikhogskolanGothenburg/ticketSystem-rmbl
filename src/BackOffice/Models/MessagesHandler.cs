using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BackOffice.Models
{
    public class MessagesHandler
    {
        public Dictionary<string, List<string>> Messages { get; private set; }
        private Sessions sessions;

        /// <summary>
        /// Constructor with TempData
        /// </summary>
        /// <param name="newTempData">TempData</param>
        public MessagesHandler(Sessions newSessions)
        {
            sessions = newSessions;

            if (sessions.Get("Messages") == null)
            {
                Messages = new Dictionary<string, List<string>>();
                sessions.Add("Messages", Messages);
            }
            else
            {
                Messages = (Dictionary<string, List<string>>)sessions.Get("Messages");
            }
        }

        /// <summary>
        /// Add a message to TempData
        /// </summary>
        /// <param name="messageType">Message type (bootstrap css-classes)</param>
        /// <param name="message">Message to print</param>
        public void Add(string messageType, string message)
        {
            if (!Messages.ContainsKey(messageType))
            {
                Messages.Add(messageType, new List<string>());
            }

            Messages[messageType].Add(message);
        }

        /// <summary>
        /// Remove all messages
        /// </summary>
        public void RemoveAll()
        {
            Messages = new Dictionary<string, List<string>>();
            sessions.Add("Messages", Messages);
        }
    }
}
