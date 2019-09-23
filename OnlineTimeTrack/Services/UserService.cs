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
                {
                      new Claim(ClaimTypes.Name, user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            /*  var claims = new List<Claim>();
             // user Id
              claims.Add(new Claim(nameof(OnlineTimeTrackClaims.UserID), user.UserID.ToString()));

             new Claim(ClaimTypes.Name, user.UserID.ToString());


             var tokenDescriptor = new SecurityTokenDescriptor
             {
                 Subject = new ClaimsIdentity(claims),
 #if DEBUG
                 Expires = DateTime.UtcNow.AddDays(1000),
 #else
                 Expires = DateTime.UtcNow.AddDays(7),
 #endif
                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
             };*/

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



         public async Task<User> Login(string Username, string Password)
         {
            // find the user from db with the same username
            
            var user =_onlineTimeTrackContext.Users.FirstOrDefault(x => x.Username == Username); 

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
            /*  else
              {
                  throw new Exception("The credentials do not match.");
              }*/

            return user;
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
            // generate password key
            user.PasswordKey = GeneratePasswordKey();

            // generate password hash
            user.PasswordHash = GenerateHash(user.PasswordHash, user.PasswordKey);

            

            // token generation
            GenerateUserToken(user);

            // otherwise, save the user to db
            //var addedUser = await _onlineTimeTrackContext.Users.AddAsync(user);


            _onlineTimeTrackContext.Users.Add(user);
            _onlineTimeTrackContext.SaveChanges();


            return user;
            

            
        }
        

        public int? GetUserIDFromContext(HttpContext context)
        {
            var userIDClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (userIDClaim != null) return Int32.Parse(userIDClaim.Value);

            return null;
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



























/*private string GeneratePasswordKey()
{
    using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
    {
        byte[] tokenData = new byte[32];
        rng.GetBytes(tokenData);

        string Token = Convert.ToBase64String(tokenData);

        return Token;
    }
}*/


