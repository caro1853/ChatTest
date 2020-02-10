using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ISessionService _sessionService;
        public RoomService(IRoomRepository roomRepository, ISessionService sessionService)
        {
            _roomRepository = roomRepository;
            _sessionService = sessionService;
        }

        public void Create(Room room)
        {
            var entity = _roomRepository.GetByName(room.Name);
            if (entity == null)
            {
                room.DateCreate = DateTime.Now;
                _roomRepository.Create(room);
            }
        }

        public IEnumerable<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public IEnumerable<Room> GetByUser(int userId)
        {
            var sessions = _sessionService.GetByUser(userId);

            List<Room> rooms =
                (from r in sessions
                 group r by (r.Room.RoomId, r.Room.Name) into g
                 select new Room
                 {
                     RoomId = g.Key.RoomId,
                     Name = g.Key.Name
                 }).ToList();


            return rooms;
        }
    }
}
