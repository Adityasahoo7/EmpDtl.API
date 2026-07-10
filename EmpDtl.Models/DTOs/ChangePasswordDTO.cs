using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string currentpassword { get; set; }

        [Required]
        public string newpassword { get; set; }

        [Required]
        [Compare("newpassword")]
        public string conformpassword { get; set; }

    }
}
