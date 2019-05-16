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
//using OnlineTimeTrack.UserDtos;
// using AutoMapper;


namespace OnlineTimeTrack.Controllers
{
    [Authorize]
    //  [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
    
        //private object _mapper;

        //  private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(
            IUserService userService,
            // IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            //  _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User user)
        {
            var User = _userService.Authenticate(user.Username, user.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            AppSettings _appSettings1 = _appSettings;
            var key = Encoding.ASCII.GetBytes(_appSettings1.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
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
                Id = user.UserID,
                Username = user.Username,
                FullName = user.FullName,
                Token = tokenString
            });
        }




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
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
           // var user = _mapper.Map<IList<UserDto>>(user);
            return Ok(User);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            //var user = _mapper.Map<User>(user);
            return Ok(User);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]User user)
        {
           // object _mapper = null;
            // map dto to entity and set id
           // var User = _mapper.Map<User>(user);
            user.UserID = id;

            try
            {
                // save 
                _userService.Update(User, user.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
            
 
    

    
