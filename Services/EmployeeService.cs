using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Services
{
    public class EmployeeService
    {
        private readonly DbConnectionFactory _db;

        public EmployeeService(DbConnectionFactory db)
        {
            _db = db;
        }

        public void RegisterEmployee(RegisterEmployeeRequest request)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new SqlCommand("RegisterEmployee", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", request.Name);
            cmd.Parameters.AddWithValue("@Designation", (object?)request.Designation ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)request.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Department", (object?)request.Department ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@JoiningDate", request.JoiningDate);
            cmd.Parameters.AddWithValue("@SkillSet", (object?)request.SkillSet ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@Username", request.Username);
           
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

            cmd.Parameters.AddWithValue("@Role", request.Role);
            cmd.Parameters.AddWithValue("@CreatedBy", request.CreatedBy);

           
            if (string.IsNullOrWhiteSpace(request.ProfilePhoto))
            {
                cmd.Parameters.Add("@ProfilePhoto", SqlDbType.VarBinary, -1).Value = DBNull.Value;
            }
            else
            {
                byte[] photoBytes = Convert.FromBase64String(request.ProfilePhoto);
                cmd.Parameters.Add("@ProfilePhoto", SqlDbType.VarBinary, -1).Value = photoBytes;
            }

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public (List<EmployeeDetailDto> Employees, int TotalCount)
            GetAllEmployees(int pageNumber, int pageSize, string? search)
        {
            var employees = new List<EmployeeDetailDto>();
            int totalCount = 0;

            using var conn = _db.CreateConnection();
            using var cmd = new SqlCommand("GetAllEmployees", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            if (string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("@Search", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Search", search);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new EmployeeDetailDto
                {
                    EmployeeId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Designation = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Address = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Department = reader.IsDBNull(4) ? null : reader.GetString(4),
                    JoiningDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                    SkillSet = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Username = reader.GetString(7),
                    Role = reader.GetString(8),
                    Status = reader.GetString(9)
                });
            }

            if (reader.NextResult() && reader.Read())
            {
                totalCount = reader.GetInt32(0);
            }

            return (employees, totalCount);
        }

        public EmployeeDetailDto? GetEmployeeById(int employeeId)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new SqlCommand("GetEmployeeById", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new EmployeeDetailDto
            {
                EmployeeId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Designation = reader.IsDBNull(2) ? null : reader.GetString(2),
                Address = reader.IsDBNull(3) ? null : reader.GetString(3),
                Department = reader.IsDBNull(4) ? null : reader.GetString(4),
                JoiningDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                SkillSet = reader.IsDBNull(6) ? null : reader.GetString(6),
                Username = reader.GetString(7),
                Role = reader.GetString(8),
                Status = reader.GetString(9),
                CreatedBy = reader.IsDBNull(10) ? null : reader.GetString(10),
                CreatedAt = reader.GetDateTime(11),
                ModifiedBy = reader.IsDBNull(12) ? null : reader.GetString(12),
                ModifiedAt = reader.IsDBNull(13) ? null : reader.GetDateTime(13),

                // Added Photo Mapping
                ProfilePhoto = reader.FieldCount > 14 && !reader.IsDBNull(14)
                    ? (byte[])reader.GetValue(14)
                    : null
            };
        }

        //Photo
        public void UpdateEmployeePhoto(int employeeId, byte[] photo)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new SqlCommand("UpdateEmployeePhoto", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@ProfilePhoto", photo);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateEmployeeByAdmin(int employeeId, UpdateEmployeeRequest request)
        {
            using var conn = _db.CreateConnection();
            using var cmd = new SqlCommand("UpdateEmployeeByAdmin", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@Name", request.Name);
            cmd.Parameters.AddWithValue("@Designation", request.Designation);
            cmd.Parameters.AddWithValue("@Address", request.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Department", request.Department);
            cmd.Parameters.AddWithValue("@JoiningDate", request.JoiningDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SkillSet", request.SkillSet ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Role", request.Role);
            cmd.Parameters.AddWithValue("@Status", request.Status);
            cmd.Parameters.AddWithValue("@ModifiedBy", request.ModifiedBy);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
