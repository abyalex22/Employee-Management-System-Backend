This repository contains the backend API for an Employee Management System. It is built using ASP.NET Core and provides a comprehensive set of endpoints for managing employee data, including authentication, role-based access control, and profile management. The system features two distinct API versions: v1, which uses ADO.NET and T-SQL stored procedures for data access, and v2, which leverages Entity Framework Core.

Features
Authentication: Secure user login using JWT (JSON Web Tokens).
Role-Based Access Control: Differentiates between 'Admin' and 'Employee' roles, providing distinct permissions.
Employee Management (Admin):
View a paginated and searchable list of all employees.
Register new employees.
Update any employee's details, role, and status.
Soft delete (deactivate) an employee's account.
Profile Management (Employee):
View and update their own profile information.
Upload a profile photo.
Dual API Versions:
v1: Utilizes raw ADO.NET with stored procedures for direct database interaction.
v2: Employs Entity Framework Core for an ORM-based approach to data access.
Technology Stack
Framework: ASP.NET Core
Language: C#
Database: Microsoft SQL Server
Data Access:
ADO.NET with Stored Procedures (API v1)
Entity Framework Core (API v2)
Authentication: JWT (JSON Web Tokens)
Password Hashing: BCrypt.Net
API Documentation: Swashbuckle (Swagger)
Setup and Installation
Prerequisites
.NET SDK (version compatible with the project)
Microsoft SQL Server (or SQL Express)
1. Database Setup
Open your SQL Server management tool (like SSMS or Azure Data Studio).
Create a new database named EmployeeManagement.
Execute the script provided in the EmployeeManagementDb.sql file against the new database. This will create the Employees table, associated stored procedures, and seed it with initial admin and user data.
2. Configure Application
Clone this repository:
git clone https://github.com/abyalex22/Employee-Management-System-Backend.git
Navigate to the project directory:
cd Employee-Management-System-Backend
Open appsettings.json.
Update the DefaultConnection string to point to your SQL Server instance.
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_INSTANCE;Database=EmployeeManagement;Trusted_Connection=True;TrustServerCertificate=True;"
},
The Jwt:Key is also in appsettings.json. For production, it is highly recommended to move this to a secure location like User Secrets or Azure Key Vault.
3. Run the Application
Restore the dependencies:
dotnet restore
Run the application:
dotnet run
The API will be available at http://localhost:5041 and https://localhost:7272. You can access the Swagger UI for testing at https://localhost:7272/swagger.
API Endpoints
The API is versioned, with v1 using ADO.NET/Stored Procedures and v2 using Entity Framework Core.

Authentication (/api/v2/auth)
| Method | Endpoint | Description | | :----- | :------------ | :-------------------- | | POST | /login | Authenticates a user and returns a JWT. |

Employees (/api/v2/employees)
| Method | Endpoint | Description | Access | | :------- | :---------------- | :--------------------------------------------------------------------------- | :---------- | | POST | / | Creates a new employee. | Public | | GET | / | Retrieves a paginated and searchable list of employees. | Admin | | GET | /{id} | Retrieves a specific employee by their ID. | Authorized | | PUT | /{id} | Updates a specific employee's details (Admin only). | Admin | | PUT | /self | Allows an employee to update their own profile. | Employee | | PUT | /{id}/photo | Updates or adds a profile photo for an employee. | Authorized | | DELETE | /{id} | Soft deletes (sets status to 'Inactive') an employee. | Admin |

(Note: v1 endpoints under /api/auth and /api/employees exist and follow a similar structure but are tied to the ADO.NET implementation.)
