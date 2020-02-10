using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(User user)
        {
            user.DateCreate = DateTime.Now;
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
        }

        public ICollection<User> GetAll()
        {
            return _appDbContext.Users.ToList();
        }
    }
}
