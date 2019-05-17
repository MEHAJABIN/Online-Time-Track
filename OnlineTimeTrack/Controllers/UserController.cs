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
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;


namespace OnlineTimeTrack.Controllers
{
    [Authorize]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
      //  private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(
            IUserService userService,
           // IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
           // _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User user)
        {
            var User = _userService.Authenticate(user.Username, user.Password);

            if (User == null)
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
            return Ok(new
            {
                Id = User.UserID,
                Username = User.Username,
                FullName = User.FullName,
               
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





    }

    internal class _appSettings
    {
    }
}

            
 
    

    
