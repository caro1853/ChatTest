using chat.core.Entities;
using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.core.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Create(User user)
        {
            user.DateCreate = DateTime.Now;
            _userRepository.Create(user);
        }

        public ICollection<User> GetAll()
        {
            return _userRepository.GetAll();
        }
    }
}
