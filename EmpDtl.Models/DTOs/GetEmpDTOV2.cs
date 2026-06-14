using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpDtl.Models.DTOs
{
    public class GetEmpDTOV2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int ManagerId { get; set; }
        public DateOnly Joindate { get; set; }
        public string? ResumePath { get; set; }
        public string? ResemefileName { get; set; }
    }
}
