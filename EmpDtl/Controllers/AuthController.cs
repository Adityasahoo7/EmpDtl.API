using EmpDtl.Models;
using Microsoft.AspNetCore.Mvc;

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


        //Add JWT Token Validation Method

        private string Generatetoken(User user)
        {
            
        }


    }
}
