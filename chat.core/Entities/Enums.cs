using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Entities
{
    public class Enums
    {
        public enum TypeObject
        {
            Room = 1,
            NormalMessage = 2,
            NewUser = 3,
            Rejected = 4,
            ConnectedUsers = 5,
            DisconnectUser = 6,
            FromServer = 7
        }

    }
}
