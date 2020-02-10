using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Interfaces
{
    public interface ISessionService
    {
        void Create(Session session);
        IEnumerable<Session> GetByUser(int userId);
        IEnumerable<Session> GetByUserRoom(int userId, int roomId);
    }
}
