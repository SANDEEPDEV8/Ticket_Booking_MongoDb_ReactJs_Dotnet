using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovieTicketsAPI.Data;
using MovieTicketsAPI.Models;
using MovieTicketsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTicketsAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MovieDbContext _dbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public IMyUserRepository _myUserRepository { get; }

        public UsersController(MovieDbContext dbContext, IConfiguration configuration, IMyUserRepository myUserRepository)
        {
            _configuration = configuration;
            _myUserRepository = myUserRepository;
            _auth = new AuthService(_configuration);
            _dbContext = dbContext;
        }

        //[HttpPost]
        //public IActionResult Register([FromBody] User user)
        //{
        //   var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();
        //    if (userWithSameEmail != null)
        //    {
        //        return BadRequest("User with same email already exists");
        //    }
        //    var userObj = new User
        //    {
        //        Name = user.Name,
        //        Email = user.Email,
        //        Password = SecurePasswordHasherHelper.Hash(user.Password),
        //        Role = "Users"
        //    };
        //    _dbContext.Users.Add(userObj);
        //    _dbContext.SaveChanges();
        //    return StatusCode(StatusCodes.Status201Created);
        //}



        //[HttpPost]
        //public IActionResult Login([FromBody] User user)
        //{
        //   var userEmail = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
        //    if (userEmail == null)
        //    {
        //        return NotFound();
        //    }
        //    if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
        //    {
        //        return Unauthorized();
        //    }

        //    var claims = new[]
        //        {
        //           new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //           new Claim(ClaimTypes.Email, user.Email),
        //           new Claim(ClaimTypes.Role, userEmail.Role)
        //         };
        //    var token = _auth.GenerateAccessToken(claims);
        //    return new ObjectResult(new
        //    {
        //        access_token = token.AccessToken,
        //        expires_in = token.ExpiresIn,
        //        token_type = token.TokenType,
        //        creation_Time = token.ValidFrom,
        //        expiration_Time = token.ValidTo,
        //        user_id = userEmail.Id,
        //        user_role = userEmail.Role,
        //    });

        //}

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] MyUser user)
        {
            var userEmail = await _myUserRepository.GetUserByEmail(email: user.Email);
            if (userEmail == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, userEmail.Role)
                 };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id,
                user_role = userEmail.Role,
            });

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] MyUser user)
        {
            var userWithSameEmail = await _myUserRepository.GetUserByEmail(email: user.Email);
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists");
            }
            var userObj = new MyUser
            {
                Name = user.Name,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                Role = "Users"
            };

            await _myUserRepository.CreateUser(userObj);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
