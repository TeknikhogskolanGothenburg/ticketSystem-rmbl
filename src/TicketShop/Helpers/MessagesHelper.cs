using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketShop.Helpers
{
    public static class MessagesHelper
    {
        /// <summary>
        /// Print Bootstrap Alerts messages
        /// </summary>
        /// <param name="type">Bootstrap css-class extension (string)</param>
        /// <param name="messages">Messages to print (List of strings)</param>
        /// <returns>Html to print</returns>
        public static HtmlContentBuilder Alerts(Dictionary<string, List<string>> messages)
        {
            HtmlContentBuilder html = new HtmlContentBuilder();

            if (messages != null && messages.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> pair in messages)
                {
                    html.AppendHtml(Alert(pair.Key, pair.Value));
                }
            }

            return html;
        }

        /// <summary>
        /// Print Bootstrap Alert message
        /// </summary>
        /// <param name="type">Bootstrap css-class extension (string)</param>
        /// <param name="messages">Messages to print (List of strings)</param>
        /// <returns>Html to print</returns>
        public static HtmlContentBuilder Alert(string type, List<string> messages)
        {
            HtmlContentBuilder html = new HtmlContentBuilder();

            if (messages != null && messages.Count > 0)
            {
                TagBuilder div = new TagBuilder("div");
                div.AddCssClass("alert alert-" + type);
                div.Attributes.Add("role", "alert");
                div.Attributes.Add("id", "alert-" + type);

                TagBuilder ul = new TagBuilder("ul");

                foreach (string message in messages)
                {
                    TagBuilder li = new TagBuilder("li");
                    li.InnerHtml.Append(message);
                    ul.InnerHtml.AppendHtml(li);
                }

                div.InnerHtml.AppendHtml(ul);
                html.AppendHtml(div);
            }

            return html;
        }

        /// <summary>
        /// Print Bootstrap Alert message
        /// </summary>
        /// <param name="type">Bootstrap css-class extension (string)</param>
        /// <param name="message">Message to print (html as string)</param>
        /// <returns>Html to print</returns>
        public static HtmlContentBuilder Alert(string type, string message)
        {
            HtmlContentBuilder html = new HtmlContentBuilder();

            if (!String.IsNullOrEmpty(message))
            {
                TagBuilder div = new TagBuilder("div");
                div.AddCssClass("alert alert-" + type);
                div.Attributes.Add("role", "alert");
                div.Attributes.Add("id", "alert-" + type);

                div.InnerHtml.AppendHtml(message);
                html.AppendHtml(div);
            }

            return html;
        }
    }
}
