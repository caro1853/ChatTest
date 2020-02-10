using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public int SessionId { get; set; }
        public virtual Session Session { get; set; }
        public DateTime DateSend { get; set; }
    }
}
