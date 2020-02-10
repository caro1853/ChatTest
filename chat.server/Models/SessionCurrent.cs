using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat.server.Models
{
    public class SessionCurrent
    {
        public Socket ObjSocket { get; set; }
        public string NickName { get; set; }
        public string Room { get; set; }
        public int SessionId { get; set; }
    }
}
