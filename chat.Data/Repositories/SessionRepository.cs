using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Data.Repositories
{
    public class SessionRepository: ISessionRepository
    {
        private readonly AppDbContext _appDbContext;

        public SessionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(Session session)
        {
            using (var dbContextTransaction = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    //Create user
                    session.User.DateCreate = DateTime.Now;
                    _appDbContext.Users.Add(session.User);
                    _appDbContext.SaveChanges();

                    //Create session
                    session.DateCreate = DateTime.Now;
                    _appDbContext.Sessions.Add(session);
                    _appDbContext.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch(Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
                
        }

        public IEnumerable<Session> GetAll()
        {
            return _appDbContext.Sessions.Include("Room").AsEnumerable();
        }
    }
}
