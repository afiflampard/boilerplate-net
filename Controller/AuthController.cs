using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Boilerplate.Data;
using Boilerplate.DTOs;
using Boilerplate.Helper;
using Boilerplate.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDBContext context, IConfiguration config){
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequst requst){

            var user = _context.Users.SingleOrDefault(x => x.Username == requst.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(requst.Password, user.Password)){
                return Unauthorized("Invalid credentials");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            return Ok(ApiResponse<object>.SuccessResponse(new {token = tokenString}));
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request){

            var user = _context.Users.SingleOrDefault(x => x.Username == request.Username);

            if(user != null){
                return Unauthorized("Invalid credentials");
            }
            var newUser = _context.Users.Add(new User{
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            });

            _context.SaveChanges();

            return Ok(ApiResponse<object>.SuccessResponse(newUser));
        }
    }
}