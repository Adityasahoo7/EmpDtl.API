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

        [HttpGet]
        [Route("getempbyid/{id}")]
        public async Task<IActionResult> getbyid(int id) {

            var emp = await _context.EmpDtlDS.FindAsync(id);

            if(emp == null)
            {
                return NotFound($"No data Avaible in this id: {id} ");
            }

            var result = new GetEmpDTO
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                Department = emp.Department,
                Designation = emp.Designation,
                ManagerId = emp.ManagerId
            };

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


        [HttpPut]
        [Route("updateemp/{id}")]
        public async Task<IActionResult> updateemp(int id , UpdateEmpDTO dto)
        {
            var emp = await _context.EmpDtlDS.FindAsync(id);

            if(emp == null)
            {
                return NotFound($"No data Avaible in this id: {id} ");
            }
            if(dto.Name != null)
            {
                emp.Name = dto.Name;
            }
            if(dto.Email != null)
            {
                emp.Email = dto.Email;
            }
            if(dto.Phone != null)
            {
                emp.Phone = dto.Phone;
            }
            if(dto.Department != null)
            {
                emp.Department = dto.Department;
            }
            if (dto.Salary != null)
            {
                emp.Salary = dto.Salary;
            }


            await _context.SaveChangesAsync();
            return Ok(emp);


        }

        [HttpDelete]
        [Route("Deleteemp/{id}")]
        public async Task<IActionResult> deleteemp(int id)
        {
            var emp = await _context.EmpDtlDS.FindAsync(id);

            if(emp == null)
            {
                return NotFound($"No data Avaible in this id: {id} ");
            }

            _context.EmpDtlDS.Remove(emp);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }

    }
}
