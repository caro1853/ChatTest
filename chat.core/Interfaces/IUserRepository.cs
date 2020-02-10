using chat.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Interfaces
{
    public interface IUserRepository
    {
        void Create(User user);
        ICollection<User> GetAll();
    }
}
