using EmpDtl.Models;
using EmpDtl.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmpDtl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDBContext _context;

        public EmployeeController(EmployeeDBContext context)
        {
                _context = context;
        }

        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<IActionResult> AddEmployee(CreateEmpDTO dto)
        {
            var emp = new AddEmployee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary,
                ManagerId = dto.ManagerId
            };

            await _context.EmpDtlDS.AddAsync(emp);
            await _context.SaveChangesAsync();

            return Ok(emp);

        }


    }
}
