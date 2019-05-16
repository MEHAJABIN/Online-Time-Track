using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Services
{
    public interface IUserService
        {
            Task<User> RegisterUser(User user);
        object Authenticate(string username, string password);
        void Update(ClaimsPrincipal user, string password);
        void Delete(int id);
        object GetAll();
        object GetById(int id);
    }
    
}
