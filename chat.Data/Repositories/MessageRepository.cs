using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Data.Repositories
{
    public class MessageRepository: IMessageRepository
    {
        private readonly AppDbContext _appDbContext;

        public MessageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(Message message)
        {
            message.DateSend = DateTime.Now;
            _appDbContext.Messages.Add(message);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Message> GetAll()
        {
            return _appDbContext.Messages.Include("Session").Include("Session.User").AsEnumerable();
        }
    }
}
