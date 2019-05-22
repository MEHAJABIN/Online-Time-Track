using OnlineTimeTrack.Contexts;
using OnlineTimeTrack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models.Data_Manager
{
    public class UserManager :IUserService<User>
    {
       readonly OnlineTimeTrackContext _onlineTimeTrackContext;

        public UserManager(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }

        public IEnumerable<User> GetAll()
        {
            return _onlineTimeTrackContext.Users.ToList();
        }

        public User Get(long id)
        {
            return _onlineTimeTrackContext.Users.FirstOrDefault(e => e.UserID== id);
        }

        public void Add(User entity)
        {
            _onlineTimeTrackContext.Users.Add(entity);
            _onlineTimeTrackContext.SaveChanges();
        }

        public void Update(User user, User entity)
        {
            user.FullName = entity.FullName;
            user.Address = entity.Address;
            user.Gender = entity.Gender;
            user.Email = entity.Email;
            user.Dob = entity.Dob;
            user.ContactNumber = entity.ContactNumber;
            user.Username = entity.Username;
          //  user.Password = entity.Password;

            _onlineTimeTrackContext.SaveChanges();
        }

        public void Delete(User user)
        {
            _onlineTimeTrackContext.Users.Remove(user);
            _onlineTimeTrackContext.SaveChanges();
        }

    }
}
