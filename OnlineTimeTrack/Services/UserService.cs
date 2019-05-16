using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OnlineTimeTrack.Contexts;
using OnlineTimeTrack.Models;
using System.Security.Claims;

namespace OnlineTimeTrack.Services
{
    public class UserService : IUserService
    {
       
        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;

        public UserService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }

        public object Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public object GetAll()
        {
            throw new NotImplementedException();
        }

        public object GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> RegisterUser(User user)
        {
            // password validation
            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8)
            {
                throw new Exception("The password provided is not valid.");
            }

            // username validation
            Regex userNameRegex = new Regex("[a-zA-Z0-9]+");
            if (string.IsNullOrWhiteSpace(user.Username) || !userNameRegex.IsMatch(user.Username))
            {
                throw new Exception("Username is not valid, it should only contain alphanumeric characters.");
            }

            // email validation
            Regex emailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase);
            if (string.IsNullOrWhiteSpace(user.Email) || !emailRegex.IsMatch(user.Email))
            {
                throw new Exception("Please provide a valid email address.");
            }


            if (_onlineTimeTrackContext.Users.Count() > 0)
            {
                User sameUser = null;
                // find if a user exists with the same user name
                sameUser = await _onlineTimeTrackContext.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

                if (sameUser != null)
                {
                    throw new Exception("A user with the same username already exists.");
                }

                // find if a user exists with the same email
                sameUser = await _onlineTimeTrackContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

                if (sameUser != null)
                {
                    throw new Exception("A user with the same email already exists.");
                }
            }

            // password hash generation
            user.PasswordKey = GeneratePasswordKey();

            // password  hash
            user.Password = HashPassword(user.Password, user.PasswordKey);

            // save the user
            var addedUser = await _onlineTimeTrackContext.AddAsync(user);
            await _onlineTimeTrackContext.SaveChangesAsync();
            addedUser.Entity.Password = null;
            addedUser.Entity.PasswordKey = null;

            // return the user
            return addedUser.Entity;
        }

        public void Update(ClaimsPrincipal user, string password)
        {
            throw new NotImplementedException();
        }

        private string GeneratePasswordKey()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                string token = Convert.ToBase64String(tokenData);

                return token;
            }
        }

        private string HashPassword(string password, string key)
        {
            string HashVal = password + key;
            using (SHA512 shaM = new SHA512Managed())
            {
                string Hash = Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(HashVal)));
                return Hash;
            }
        }
    }
}

