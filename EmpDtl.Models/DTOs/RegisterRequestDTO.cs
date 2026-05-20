using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Models.DTOs
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
