using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Services;
using Microsoft.Data.SqlClient;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterEmployeeRequest request)
        {
            try
            {
                _employeeService.RegisterEmployee(request);
                return Ok("Employee registered successfully");
            }
            catch (SqlException ex) when (ex.Message.Contains("Username already exists"))
            {
                return Conflict("Username already exists");
            }
        }
        [HttpGet]
        public IActionResult GetAllEmployees(
    int pageNumber = 1,
    int pageSize = 5,
    string? search = null)
        {
            var result = _employeeService.GetAllEmployees(pageNumber, pageSize, search);

            return Ok(new
            {
                data = result.Employees,
                totalCount = result.TotalCount
            });
        }


        [HttpGet("{employeeId:int}")]
        public IActionResult GetEmployeeById(int employeeId)
        {
            var employee = _employeeService.GetEmployeeById(employeeId);

            if (employee == null)
                return NotFound("Employee not found or inactive");

            return Ok(employee);
        }
        [HttpPut("{employeeId:int}")]
        public IActionResult UpdateEmployeeByAdmin(int employeeId,UpdateEmployeeRequest request)
        {
            try
            {
                _employeeService.UpdateEmployeeByAdmin(employeeId, request);
                return Ok("Employee updated successfully");
            }
            catch (SqlException ex) when (ex.Message.Contains("Employee not found"))
            {
                return NotFound("Employee not found");
            }
        }

        //Photo Update
        [HttpPut("{id}/photo")]
        public IActionResult UpdatePhoto(int id, [FromBody] PhotoUploadRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Photo))
                return BadRequest("Photo required");

            try
            {
                var base64 = request.Photo.Contains(",")
                    ? request.Photo.Split(',')[1]
                    : request.Photo;

                var bytes = Convert.FromBase64String(base64);

                _employeeService.UpdateEmployeePhoto(id, bytes);

                return Ok();
            }
            catch (FormatException)
            {
                return BadRequest("Invalid base64 image format.");
            }
        }




    }
}
