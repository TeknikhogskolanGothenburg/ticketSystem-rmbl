using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BackOffice.Models
{
    public class MessagesHandler
    {
        private ITempDataDictionary tempData;

        /// <summary>
        /// Getter for TempData["Messages"]
        /// </summary>
        public Dictionary<string, List<string>> Messages
        {
            get
            {
                return ((Dictionary<string, List<string>>)tempData["Messages"]);
            }
        }

        /// <summary>
        /// Constructor with TempData
        /// </summary>
        /// <param name="newTempData">TempData</param>
        public MessagesHandler(ITempDataDictionary newTempData)
        {
            tempData = newTempData;
            if (tempData["Messages"] == null)
            {
                tempData["Messages"] = new Dictionary<string, List<string>>();
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
            tempData["Messages"] = new Dictionary<string, List<string>>();
        }
    }
}
