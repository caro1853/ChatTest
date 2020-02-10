using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
