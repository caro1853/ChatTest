using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Data
{
    public class AppDbContext: DbContext
    {
        
        public AppDbContext()
            : base("name=ConnectionDefault")
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
