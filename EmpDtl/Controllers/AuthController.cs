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
        private readonly ILogger<AuthController> _logger;
        public AuthController(IConfiguration config,EmployeeDBContext  context,ILogger<AuthController> logger)
        {
            _configuration = config;
            _context = context;
            _logger = logger;
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
                _logger.LogInformation("No Valid Username Given");
                return NotFound("User Not Found");
            }

            bool ispassvalid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.Passwordhash
                );

            if (!ispassvalid)
            {
                _logger.LogInformation("No Valid Password Given");
                return Unauthorized("Enter Valid Password");
            }

            var token = Generatetoken(user);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            };

            _logger.LogInformation("Login Successful for userName : "+user.Username);

            return Ok(response);
        }

        [HttpPost]
        [Route("ChnagePassword")]
       // [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> changepassword(ChangePasswordDTO dto)
        {
            var userclaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userclaim)|| !Guid.TryParse(userclaim ,out Guid userId))
            {
                return Unauthorized(
                    new
                    {
                        message = "Invalid User"
                    }
                    );
            }

            var user = await _context.UserDs.FirstOrDefaultAsync(x => x.Id == userId);
            if(user == null)
            {
                return NotFound(

                    new
                    {
                        message = "User Not found"
                    });
            }


            if (!BCrypt.Net.BCrypt.Verify(dto.currentpassword, user.Passwordhash))
            {
                return BadRequest(new
                {
                    message = "New Password not be same as old password"
                });
            }

            user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.currentpassword);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Password Change Successfully"
            });


        
        
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
                _logger.LogInformation(checkuser.Username+" User Already Present but try to register again");
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

            _logger.LogInformation("User Creation Scuuessfully For UserName " + user.Username + " For Role : " + user.Role);
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
            _logger.LogInformation("Token Created Successfully for UserName: " + user.Username);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
