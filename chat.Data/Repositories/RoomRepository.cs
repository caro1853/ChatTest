using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _appDbContext;

        public RoomRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(Room room)
        {
            room.DateCreate = DateTime.Now;
            _appDbContext.Rooms.Add(room);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Room> GetAll()
        {
            return _appDbContext.Rooms.ToList();
        }        

        public Room GetByName(string name)
        {
            return _appDbContext.Rooms.Where(p => p.Name == name).FirstOrDefault();
        }
    }
}
