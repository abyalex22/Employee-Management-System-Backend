using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeManagement.API.Data;

namespace EmployeeManagement.API.Services
{
    public class AuthService
    {
        private readonly DbConnectionFactory _db;

        public AuthService(DbConnectionFactory db)
        {
            _db = db;
        }

        public (int employeeId, string role, string status, string storedPassword)?
            ValidateLogin(string username)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new SqlCommand("ValidateLogin", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return (
                employeeId: reader.GetInt32(0),   // EmployeeId
                role: reader.GetString(3),  // Role
                status: reader.GetString(4),  // Status
                storedPassword: reader.GetString(2) // Password
            );
        }
    }
}
