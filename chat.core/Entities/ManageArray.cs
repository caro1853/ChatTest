using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Entities
{
    public class ManageArray
    {
        public byte[] buffer { get; set; }
        public bool InUse { get; set; }
        public MessageTransport MessageTransport { get; set; }

    }
}
