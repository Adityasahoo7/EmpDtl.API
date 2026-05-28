using EmpDtl.Models;
using EmpDtl.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmpDtl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDBContext _context;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(EmployeeDBContext context ,ILogger<EmployeeController> logger)
        {
                _context = context;
            _logger = logger;
        }

        private string username => User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        private string role => User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";

        [AllowAnonymous]
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
            _logger.LogInformation("Use GETALL API by username : " + username+" With Role: "+role);
            return Ok(result);
        }

        [HttpGet] 
        [Route("GetAllEmpwithResume")]
        public async Task<IActionResult> getv2()
        {
            var emp = await _context.EmpDtlDS.ToListAsync();
            var result = emp.Select(e => new GetEmpDTOV2
            {
                Id=e.Id,
                Name=e.Name,
                Email=e.Email,
                Department=e.Department,
                Designation=e.Designation,
                ManagerId = e.ManagerId,
                ResumePath = e.ResumePath,
                ResemefileName=e.ResemefileName
            });
            _logger.LogInformation("Username : "+username+" Have use The GETALLEMP V2");
            return Ok(result);

        }


        [Authorize(Roles ="Admin,User")]
        [HttpGet]
        [Route("getempbyid/{id}")]
        public async Task<IActionResult> getbyid(int id) {

            var emp = await _context.EmpDtlDS.FindAsync(id);

            if(emp == null)
            {
                _logger.LogInformation("Username : " + username + " Have enter wrong id to search employee");
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

            _logger.LogInformation("Username : " + username + " Have get the user details of this ID : " + id);
            return Ok(result);


        
        
        }

        [Authorize(Roles = "Admin,User")]
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

            _logger.LogInformation("Username : " + username + " Have create a user with this name :" + dto.Name);
            return Ok(emp);

        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        [Route("updateemp/{id}")]
        public async Task<IActionResult> updateemp(int id , UpdateEmpDTO dto)
        {
            var emp = await _context.EmpDtlDS.FindAsync(id);

            if (emp == null)
               
            {
                _logger.LogInformation("Username : " + username + " Have enter wrong id to update ID : " + id);

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

            _logger.LogInformation("Username : " + username + " Have Update the detaails of this  ID : " + id);

            return Ok(emp);


        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Route("createemp")]
        public async Task<IActionResult> createempv2(CreateEmpDTO dto)
        {
            var emp = new AddEmployee
            {

                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                ManagerId = dto.ManagerId

            };

            await _context.EmpDtlDS.AddAsync(emp);
            await _context.SaveChangesAsync();

            return Ok(emp);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Deleteemp/{id}")]
        public async Task<IActionResult> deleteemp(int id)
        {
            var emp = await _context.EmpDtlDS.FindAsync(id);

            if(emp == null)
            {
                _logger.LogInformation("Username : " + username + " Have enter wrong id to Delete ID : " + id);


                return NotFound($"No data Avaible in this id: {id} ");
            }

            _context.EmpDtlDS.Remove(emp);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Username : " + username + " Have delete the details of this ID : " + id);



            return Ok("Deleted Successfully");
        }


        //UPLODE RESUME ACTION METHOD

        [HttpPost]
        [Route("create-employee-resume")]
        public async Task<IActionResult> CreateEmpwithResume([FromForm] CreateEmpDTOV2 dto)
        {
            var emp = new AddEmployee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Designation = dto.Designation,
                Department = dto.Department,
                Salary = dto.Salary,
                ManagerId = dto.ManagerId
            };

            //UPLODE  RESUME
            if (dto.Resume != null)
            {
                var folderpath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uplodes",
                    "Resumes"
                    );


                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                var extention = Path.GetExtension(dto.Resume.FileName);

                var uniquefilename = Guid.NewGuid().ToString() + extention;

                var filepath = Path.Combine(folderpath, uniquefilename);


                using(var stream = new FileStream(filepath, FileMode.Create))
                {
                    await dto.Resume.CopyToAsync(stream);
                }


                emp.ResumePath = $"Uplodes/Resumes/{uniquefilename}";
                emp.ResemefileName = dto.Resume.FileName;

                _logger.LogInformation("Uplode Resume for Name : " + dto.Name);



            }

            await _context.EmpDtlDS.AddAsync(emp);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Username : "+username+" Created Employee Successfully with name : "+emp.Name);
            return Ok(emp);
        }



    }
}
