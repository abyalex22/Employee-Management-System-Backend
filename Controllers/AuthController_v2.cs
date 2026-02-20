using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.ServicesV2;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.ModelsV2.DTOs;
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
            var user = await _auth.GetUserByUsername(request.Username);

            if (user == null)
                return Unauthorized("Invalid credentials");

            bool valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!valid)
                return Unauthorized("Invalid credentials");

            if (user.Status == "Inactive")
                return Unauthorized("User inactive");

            var token = GenerateToken(user.EmployeeId, user.Role);

            return Ok(new LoginResponse_v2
            {
                EmployeeId = user.EmployeeId,
                Role = user.Role,
                Status = user.Status,
                Token = token
            });
        }

        private string GenerateToken(int id, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
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
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
