using EcommerceProject.Domain.Entity;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProject.Domain.Core
{
    public class UsersDomain : IUsersDomain
    {
        private readonly IUsersRepository _userRepository;

        public UsersDomain(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User Authenticate(string username, string password)
        {
            return _userRepository.Authenticate(username, password);
        }
    }
}
