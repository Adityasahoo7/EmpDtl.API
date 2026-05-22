using BCrypt.Net;
using EmpDtl.Models;
using EmpDtl.Models.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpDtl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static List<User> usersmodel = new List<User>();
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration config)
        {
            _configuration = config;
        }

        //REGISTER METHOD
        [HttpPost]
        [Route("Registeruser")]
        public IActionResult Register(RegisterRequestDTO dto)
        {

            var existinguser = usersmodel.FirstOrDefault(
                x => x.Username == dto.Username
                );

            if(existinguser != null)
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

            usersmodel.Add(user);
            return Ok("User Created Successfuly");
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
                    _configuration["JWT.key"])
                );

            var cred = new SigningCredentials(
                key,SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT.Issuer"],
                audience: _configuration["JWT.Audience"],
                claims:claims,
                expires:DateTime.Now.AddMinutes(
                    Convert.ToDouble(
                        _configuration["JWT.Expair"]
                        )
                    ),
                signingCredentials:cred
                
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
