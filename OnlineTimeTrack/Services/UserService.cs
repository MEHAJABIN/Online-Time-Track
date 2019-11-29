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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace OnlineTimeTrack.Services
{

    public class UserService : IUserService
    {

        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;
        private readonly AppSettings _appSettings;
       


        public UserService(OnlineTimeTrackContext onlineTimeTrackContext, IOptions<AppSettings> appSettings)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
             _appSettings = appSettings.Value;
            

        }

        private void GenerateUserToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
#if DEBUG
                {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                }),
             
                Expires = DateTime.UtcNow.AddDays(7),

#endif

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);


        }

        private string GenerateHash(string Password, string PasswordKey)
        {
            var passwordDigest = Password + PasswordKey;
            using (SHA512 shaM = new SHA512Managed())
            {
                return Encoding.ASCII.GetString(
                    shaM.ComputeHash(Encoding.ASCII.GetBytes(passwordDigest)));
            }
        }

        private string GeneratePasswordKey()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&&*()_+|";
            return new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<User> Authenticate(string Username, string Password)
        {
            string passwordKey = Password;
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(passwordKey))
            {
                return null;
            }

          
             
            // find the user from db with the same username

            var user = _onlineTimeTrackContext.Users.SingleOrDefault(x => x.Username == Username);

            if (user == null)
            {
                throw new Exception("The user is not registered.");
            }

            // generate hash using the user's passwordKey and the given password
            var hash = GenerateHash(Password, user.PasswordKey);

            // see if the hash matches with the one stored in DB
            if (hash == user.PasswordHash)
            {
                // generate token
                GenerateUserToken(user);

                await _onlineTimeTrackContext.SaveChangesAsync();

            }

            return user;
        }



        public User Create(User user, string Password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(Password))
                throw new AppException("Password is required");

            if (_onlineTimeTrackContext.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            //    byte[] PasswordKey, password;
            CreatePasswordHash(user.PasswordKey, user.Password);

            user.PasswordKey = Password;
            user.Password = Password;

            _onlineTimeTrackContext.Users.Add(user);
            _onlineTimeTrackContext.SaveChanges();

            return user;
        }




        void Update(User user, string password = null)
        {
            var User = _onlineTimeTrackContext.Users.Find(user.UserID);

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
                   
             }

             private void CreatePasswordHash(string passwordKey, string password)
             {
                
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

            }


            // generate password key
            user.PasswordKey = GeneratePasswordKey();

            // generate password hash
            user.PasswordHash = GenerateHash(user.PasswordHash, user.PasswordKey);



            // token generation
            GenerateUserToken(user);

            // otherwise, save the user to db
         
            _onlineTimeTrackContext.Users.Add(user);
            _onlineTimeTrackContext.SaveChanges();

            return user;
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



        void Add(User entity)
        {
            _onlineTimeTrackContext.Users.Add(entity);
            _onlineTimeTrackContext.SaveChanges();
        }






        public async Task<User> GetById(long? id)
        {
            var result = await _onlineTimeTrackContext.Users.Where(u => u.UserID == id).ToListAsync();

            return _onlineTimeTrackContext.Users.FirstOrDefault(x => x.UserID == id);
        }




        //Update UserDetails
        public async Task<User> RegisterdUsers(User UserID)
        {
            _onlineTimeTrackContext.Users.Update(UserID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingUser = _onlineTimeTrackContext.Users.FirstOrDefault(x => x.UserID == UserID.UserID);

            return ExistingUser;
        }


        //Delete UserDetails
        public async Task<User> RegisterdUser(User UserID)
        {
            _onlineTimeTrackContext.Users.Remove(UserID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingUser = _onlineTimeTrackContext.Users.FirstOrDefault(x => x.UserID == UserID.UserID);

            return ExistingUser;
        }


        public async Task<IEnumerable<User>> GetAllUsers(int start, int limit)
        {

            if (start == 0 & limit == 0)
            {
                var user = await _onlineTimeTrackContext.Users.ToListAsync();
                return user;
            }
            else
            {
                var result = await _onlineTimeTrackContext.Users.Skip(start).Take(limit).ToListAsync();

                return result;
            }

        }

      
    }
}
























































