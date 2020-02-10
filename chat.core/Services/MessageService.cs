using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ISessionService _sessionService;
        public MessageService(IMessageRepository messageRepository, ISessionService sessionService)
        {
            _messageRepository = messageRepository;
            _sessionService = sessionService;
        }

        public void Create(Message message)
        {
            message.DateSend = DateTime.Now;
            _messageRepository.Create(message);
        }

        public IEnumerable<Message> GetByRoom(int roomId)
        {
            return _messageRepository.GetAll().Where(p=>p.Session.RoomId == roomId).AsEnumerable();
        }

        public IEnumerable<Message> GetByUserRoom(int userId, int roomId)
        {
            var sessions = _sessionService.GetByUserRoom(userId, roomId);

            var messages = _messageRepository.GetAll();

            List<Message> res =
                (from s in sessions
                join m in messages on s.SessionId equals m.SessionId
                select m).ToList();

            return res;
        }
    }
}
