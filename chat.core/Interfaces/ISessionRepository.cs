using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Interfaces
{
    public interface ISessionRepository
    {
        void Create(Session user);
        IEnumerable<Session> GetAll();
    }
}
