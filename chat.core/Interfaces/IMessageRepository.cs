using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Interfaces
{
    public interface IMessageRepository
    {
        void Create(Message user);
        IEnumerable<Message> GetAll();
    }
}
