using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
