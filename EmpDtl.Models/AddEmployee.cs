using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Models
{
    public class AddEmployee
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        [Required(ErrorMessage = "Salary is required")]
        public int Salary { get; set; }
        public int ManagerId { get; set; }
    }
}
