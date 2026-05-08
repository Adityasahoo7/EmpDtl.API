using EmpDtl.Models;
using EmpDtl.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        [HttpGet]
        [Route("Getallemp")]
        public async Task<IActionResult> get() {

            var emp = await _context.EmpDtlDS.ToListAsync();

            var result = emp.Select(e => new GetEmpDTO
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Designation = e.Designation,
                ManagerId = e.ManagerId


            });

            return Ok(result);
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
