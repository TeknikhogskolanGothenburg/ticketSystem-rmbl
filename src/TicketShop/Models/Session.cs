using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketShop.Models
{
    public class Session
    {
        private object valueData;
        public DateTime Created { get; }
        public DateTime LastUse { get; set; }
        public int Expires { get; set; }

        public DateTime ExpireDate
        {
            get
            {
                return DateTime.Now.AddMilliseconds(Expires);
            }
        }

        public bool Expired
        {
            get
            {
                return (ExpireDate <= DateTime.Now);
            }
        }

        public object Value {
            get
            {
                if (!Expired)
                {
                    LastUse = DateTime.Now;
                    return valueData;
                }
                else
                {
                    throw new TimeoutException();
                }
            }
            set
            {
                valueData = value;
            }
        }

        /// <summary>
        /// Constructor with value and expires time
        /// </summary>
        /// <param name="value">Session value</param>
        /// <param name="expires">Expires time</param>
        public Session(object value, int expires)
        {
            Created = DateTime.Now;
            LastUse = DateTime.Now;
            Expires = expires;
            Value = value;
        }
    }
}
