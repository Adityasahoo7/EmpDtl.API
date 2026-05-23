using BCrypt.Net;
using EmpDtl.Models;
using EmpDtl.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
       // private static List<User> usersmodel = new List<User>();
        private readonly IConfiguration _configuration;
        private readonly EmployeeDBContext _context;
        public AuthController(IConfiguration config,EmployeeDBContext  context)
        {
            _configuration = config;
            _context = context;
        }

        //LOGIN METHOD
        [HttpPost]
        [Route("loginuser")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var user = await _context.UserDs.FirstOrDefaultAsync(
                x=>x.Username==dto.Username
                );
            //var user = usersmodel.FirstOrDefault(
            //    x=>x.Username==dto.Username
            //    );

            if(user == null)
            {
                return NotFound("User Not Found");
            }

            bool ispassvalid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.Passwordhash
                );

            if (!ispassvalid)
            {
                return Unauthorized("Enter Valid Password");
            }

            var token = Generatetoken(user);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            };

            return Ok(response);
        }



        //REGISTER METHOD
        [HttpPost]
        [Route("Registeruser")]
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
        {
            var checkuser = await _context.UserDs.FirstOrDefaultAsync(
                x=>x.Username ==dto.Username
                );

            //var existinguser = usersmodel.FirstOrDefault(
            //    x => x.Username == dto.Username
            //    );

            if(checkuser != null)
            {
                return BadRequest("User Already Present");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };
            _context.UserDs.Add(user);
            _context.SaveChanges();

            //usersmodel.Add(user);
            return Ok("User Created Successfuly");
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public IActionResult logout()
        {
            return Ok("Logout successfully");
        }

        //Add JWT Token Validation Method

        private string Generatetoken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Username),

                new Claim(ClaimTypes.Role,user.Role),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JWT:key"])
                );

            var cred = new SigningCredentials(
                key,SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims:claims,
                expires:DateTime.Now.AddMinutes(
                    Convert.ToDouble(
                        _configuration["JWT:Expair"]
                        )
                    ),
                signingCredentials:cred
                
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
