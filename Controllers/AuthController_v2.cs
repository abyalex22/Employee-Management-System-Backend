using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.ServicesV2;
using EmployeeManagement.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/v2/auth")]
    public class AuthController_v2 : ControllerBase
    {
        private readonly AuthService_v2 _auth;
        private readonly IConfiguration _config;

        public AuthController_v2(AuthService_v2 auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _auth.ValidateLogin(request.Username);

            if (result == null)
                return Unauthorized("Invalid credentials");

            bool valid = BCrypt.Net.BCrypt.Verify(request.Password, result.Value.hash);

            if (!valid)
                return Unauthorized("Invalid credentials");

            if (result.Value.status == "Inactive")
                return Unauthorized("User inactive");

            var token = GenerateToken(result.Value.id, result.Value.role);

            return Ok(new
            {
                employeeId = result.Value.id,
                role = result.Value.role,
                status = result.Value.status,
                token
            });
        }

        private string GenerateToken(int id, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,id.ToString()),
                new Claim(ClaimTypes.Role,role)
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
