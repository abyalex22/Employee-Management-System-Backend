using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Services;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var result = _authService.ValidateLogin(request.Username);

            if (result == null)
                return Unauthorized("Invalid username or password");

            bool isValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                result.Value.storedPassword
            );

            if (!isValid)
                return Unauthorized("Invalid username or password");

            if (result.Value.status == "Inactive")
                return Unauthorized("User is inactive");

            return Ok(new
            {
                employeeId = result.Value.employeeId,
                role = result.Value.role,
                status = result.Value.status
            });
        }


    }
}
