using EmployeeManagement.API.Models;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// JWT namespaces
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // model validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

            // generate JWT token using helper method
            var token = GenerateJwtToken(result.Value.employeeId, result.Value.role);

            // return response
            return Ok(new LoginResponse
            {
                EmployeeId = result.Value.employeeId,
                Role = result.Value.role,
                Status = result.Value.status,
                Token = token
            });
        }

        // ===== JWT GENERATOR METHOD =====
        private string GenerateJwtToken(int employeeId, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employeeId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
