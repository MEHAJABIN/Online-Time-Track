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
using Microsoft.VisualStudio.Web.CodeGeneration;


namespace OnlineTimeTrack.Services
{
    public class UserService : IUserService

    {
        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;

        public UserService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }

        public User Authenticate(string Username, string Password)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return null;
            }

            var user = _onlineTimeTrackContext.Users.SingleOrDefault(x => x.Username == Username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(user.Password,user.PasswordKey,user.PasswordSalt))
            {
                return null;
            }

            // authentication successful
            return user;
        }

      

        private bool VerifyPasswordHash(string Password, string PasswordKey, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
            //    if (Password == null) throw new ArgumentNullException("Password");
            if (string.IsNullOrWhiteSpace(PasswordKey)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            // if (PasswordKey.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            //if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            // if (storedSalt.Length != 8) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            if (storedSalt.Length >= 8)
            {
                new Exception("Login Successfully");
            }


                using (HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(PasswordKey));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != PasswordKey[i]) return false;
                }
            }

            return true;
        }

        public IEnumerable<User> GetAll()
        {
            return _onlineTimeTrackContext.Users;
        }

        public User GetById(int id)
        {
            return _onlineTimeTrackContext.Users.Find(id);
        }

        public User Create(User user,string Password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(Password))
                throw new AppException("Password is required");

            if (_onlineTimeTrackContext.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

        //    byte[] PasswordKey, password;
            CreatePasswordHash( user.PasswordKey,user.Password);

            user.PasswordKey = Password;
            user.Password= Password;

            _onlineTimeTrackContext.Users.Add(user);
            _onlineTimeTrackContext.SaveChanges();

            return user;
        }

        private void CreatePasswordHash( byte[] PasswordKey,byte[] Password)
        {
            throw new NotImplementedException();
        }

        void Update(User user, string password = null)
        { var User = _onlineTimeTrackContext.Users.Find(user.UserID);

            if (user == null)
                throw new AppException("User not found");

            if (user.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_onlineTimeTrackContext.Users.Any(x => x.Username == user.Username))
                    throw new AppException("Username " + user.Username + " is already taken");
    }

    

    // update user properties
    user.FullName = user.FullName;
            user.Username = user.Username;
            
            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(user.PasswordKey);

                string passwordKey = null;
                user.PasswordKey = passwordKey;
                user.Password = password;
            }

            _onlineTimeTrackContext.Users.Update(user);
            _onlineTimeTrackContext.SaveChanges();
        }

        private void CreatePasswordHash(string passwordKey)
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(string password, string password1)
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(string password1, byte[] passwordKey, string password)
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(byte[] passwordHash, string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                NewMethod(password, hmac);
            }
        }

        private static void NewMethod(string password, HMACSHA512 hmac)
        {
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        void Delete(int id)
        {
            if (_onlineTimeTrackContext.Users.Find(id) != null)
            {
                _onlineTimeTrackContext.Users.Remove(_onlineTimeTrackContext.Users.Find(id));
                _onlineTimeTrackContext.SaveChanges();
            }
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

        void IUserService.Update(User user, string password)
        {
            throw new NotImplementedException();
        }

        void IUserService.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}




        
