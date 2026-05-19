using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Models
{
    public class User
    {
        [Required(ErrorMessage ="ID Must be Required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters.")]
        public string Username { get; set; }

        [Required]
        public string Passwordhash { get; set; }

        [Required(ErrorMessage ="Role Must be required")]
        [RegularExpression("^(User|Manager|Admin)$",
            ErrorMessage ="Role Must be user and admin")]
        public string Role { get; set; }
    }
}
