using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineTimeTrack.Contexts;
using Microsoft.AspNetCore.Http;

namespace OnlineTimeTrack.Services
{
    public interface IUserService
    {
        Task<User>RegisterUser(User user);
        Task<User>RegisterdUsers(User UserID);
        Task<User> RegisterdUser(User UserID);
        User Authenticate(string Username, string Password);
        IEnumerable<User> GetAll();
        User GetById(long id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        int? GetUserIDFromContext(HttpContext context);


    }

}






