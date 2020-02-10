using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Interfaces
{
    public interface IRoomRepository
    {
        void Create(Room room);
        IEnumerable<Room> GetAll();
        Room GetByName(string name);
        

    }
}
