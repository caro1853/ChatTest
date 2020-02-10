using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Interfaces
{
    public interface IMessageService
    {
        void Create(Message message);
        IEnumerable<Message> GetByUserRoom(int userId, int roomId);
        IEnumerable<Message> GetByRoom(int roomId);
    }
}
