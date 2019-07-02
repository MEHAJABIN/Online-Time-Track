using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineTimeTrack.Contexts;
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;


namespace OnlineTimeTrack.Controllers
{
    [Authorize]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UserController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }



        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User loginDetails)
        {
            var user = _userService.Authenticate(loginDetails.Username, loginDetails.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

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
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            user.Token = tokenString;
            return Ok(new
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Address = user.Address,
                Gender = user.Gender,
                Dob = user.Dob,
                Age = user.Age,
                ContactNumber = user.ContactNumber,
                Email = user.Email,
                Username = user.Username,
                Token = tokenString
            });

        }



        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<Response<User>> Register([FromBody] User user)
        {
            if (user == null)
            {
                return Response<User>.CreateResponse(false, "Please provide valid user data.", null);
            }

            try
            {
                var newUser = await _userService.RegisterUser(user);

                return Response<User>.CreateResponse(true, "Successfully registered.", newUser);
            }
            catch (Exception e)
            {
                return Response<User>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpGet("GetById")]
        public async Task<Response<User>> GetById([FromQuery]long? id)

        {
            if (id == null)
            {
                return Response<User>.CreateResponse(false, "Please provide valid User Id.", null);
            }

            try
            {
                var ExistingUser = await _userService.GetById(id.GetValueOrDefault());
                if (ExistingUser == null)
                {
                    return Response<User>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<User>.CreateResponse(true, "Successfully loaded.", ExistingUser);
            }
            catch (Exception e)
            {
                return Response<User>.CreateResponse(false, e.Message, null);
            }
        }


        //For Update UserDetails
        [HttpPut("RegisterdUsers")]

        public async Task<Response<User>> RegisterdUsers([FromBody] User UserID)

        {
            if (UserID == null)
            {
                return Response<User>.CreateResponse(false, "Please provide valid User Id.", null);

            }
            try
            {
                var ExistingUser = await _userService.RegisterdUsers(UserID);
                if (ExistingUser == null)
                {
                    return Response<User>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<User>.CreateResponse(true, "Successfully updated.", ExistingUser);

            }
            catch (Exception e)
            {
                return Response<User>.CreateResponse(false, e.Message, null);
            }
        }


        //For Delete User
        [HttpDelete("RegisterdUser")]
        public async Task<Response<User>> RegisterdUser([FromBody] User UserID)

        {
            if (UserID == null)
            {
                return Response<User>.CreateResponse(false, "Please provide valid User Id.", null);

            }
            try
            {
                var ExistingUser = await _userService.RegisterdUser(UserID);
                if (ExistingUser == null)
                {
                    return Response<User>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<User>.CreateResponse(true, "Successfully deleted.", null);
            }
            catch (Exception e)
            {
                return Response<User>.CreateResponse(false, e.Message, null);
            }
        }




        [HttpGet("GetAllUsers")]
        public async Task<Response<IEnumerable<User>>> GetAllUsers([FromQuery] int start, int limit)
        {

            try
            {
                var users = await _userService.GetAllUsers(start, limit);

                if (users == null)
                {
                    return Response<IEnumerable<User>>.CreateResponse(false, "Not  valid ", null);
                }

                return Response<IEnumerable<User>>.CreateResponse(true, "Successfully loaded.", users);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<User>>.CreateResponse(false, e.Message, null);
            }

        }
    }
}

























































































