using Dapper.Example.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Example.Repository
{
    public class EmployeeRepositoryDapper : IEmployeeRepository
    {
        private IDbConnection db;
        public EmployeeRepositoryDapper(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DapperExample"));
        }
        public Employee Add(Employee employee)
        {
            string query = "Insert Into Employees(Name,Email,Phone,Title,CompanyId) values(@Name,@Email,@Phone,@Title,@CompanyId)" +
                "Select CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(query, new { @Name = employee.Name, @Email = employee.Email, @Phone = employee.Phone, @Title = employee.Title, @CompanyId = employee.CompanyId }).SingleOrDefault();
            employee.EmployeeId = id;
            return employee;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            string query = "Insert Into Employees(Name,Email,Phone,Title,CompanyId) values(@Name,@Email,@Phone,@Title,@CompanyId)" +
                "Select CAST(SCOPE_IDENTITY() as int)";
            var id = await db.QueryAsync<int>(query, new { @Name = employee.Name, @Email = employee.Email, @Phone = employee.Phone, @Title = employee.Title, @CompanyId = employee.CompanyId });
            employee.EmployeeId = id.SingleOrDefault();
            return employee;
        }

        public void Delete(int id)
        {
            string query = "Delete From Employees Where EmployeeId = @Id";
            db.Execute(query, new { id });
        }

        public Employee Find(int id)
        {
            string query = "Select * From Employees Where EmployeeId = @EmployeeId";
            return db.Query<Employee>(query, new { @EmployeeId = id }).SingleOrDefault();
        }

        public List<Employee> GetAll()
        {
            string query = "Select * From Employees";
            return db.Query<Employee>(query).ToList();
        }

        public bool IsExists(int id)
        {
            var employee = Find(id);
            return (employee == null);
        }

        public Employee Update(Employee employee)
        {
            string query = "Update Employees SET Name = @Name,Email = @Email,Phone = @Phone,Title = @Title,CompanyId = @CompanyId Where EmployeeId = @EmployeeId";
            db.Execute(query, employee);
            return employee;
        }
    }
}
