using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chat.core.Entities.Enums;

namespace chat.core.Entities
{
    [Serializable]
    public class MessageTransport
    {
        public MessageTransport()
        {
            LstRooms = new List<string>();
            NickName = string.Empty;
            Room = string.Empty;
            Message = string.Empty;
            ConnectedUsers = new List<Session>();
        }
        public TypeObject _typeObject { get; set; }

        public List<string> LstRooms { get; set; }

        public string NickName { get; set; }
        public string Room { get; set; }

        public string Message { get; set; }
        public List<Session> ConnectedUsers { get; set; }
    }
}
