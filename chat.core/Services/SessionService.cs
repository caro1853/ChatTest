using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IRoomRepository _roomRepository;
        public SessionService(ISessionRepository sessionRepository, IRoomRepository roomRepository)
        {
            _sessionRepository = sessionRepository;
            _roomRepository = roomRepository;
        }

        public void Create(Session session)
        {
            var room = _roomRepository.GetByName(session.Room.Name);

            if (room == null)
            {
                _roomRepository.Create(session.Room);
                session.RoomId = session.Room.RoomId;
                session.Room = null;
            }
            else
            {
                session.RoomId = room.RoomId;
                session.Room = null;
            }

            session.DateCreate = DateTime.Now;
            _sessionRepository.Create(session);
        }

        public IEnumerable<Session> GetByUser(int userId)
        {
            return _sessionRepository.GetAll().Where(p => p.UserId == userId);
        }

        public IEnumerable<Session> GetByUserRoom(int userId, int roomId)
        {
            return _sessionRepository.GetAll().Where(p => p.UserId == userId && p.RoomId == roomId);
        }
    }
}
