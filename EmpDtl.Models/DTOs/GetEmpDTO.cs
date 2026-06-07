using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Models.DTOs
{
    public class GetEmpDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
         public string Email { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int  Salary { get; set; }
        public int ManagerId { get; set; }
    }
}
