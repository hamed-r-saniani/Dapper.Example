using Dapper.Example.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.Example.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;
        public BonusRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DapperExample"));
        }

        public void AddTestRecords(Company command)
        {
            string query = "Insert Into Companies (Name,Address,City,State,PostalCode) values(@Name,@Address,@City,@State,@PostalCode);"
                + "Select CAST(SCOPE_IDENTITY() AS int);";
            int id = db.Query<int>(query,command).SingleOrDefault();
            command.CompanyId = id;

            foreach (var item in command.Employees)
            {
                item.CompanyId = command.CompanyId;
                string queryEm = "Insert Into Employees(Name,Email,Phone,Title,CompanyId) values(@Name,@Email,@Phone,@Title,@CompanyId);"
                    + "Select CAST(SCOPE_IDENTITY() AS int);";

                int employeeId =  db.Query<int>(queryEm, item).SingleOrDefault();
            }
        }

        public List<Company> GetAllCompanyWithEmployees()
        {
            string query = "Select C.*,E.* From Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";

            var companyDic = new Dictionary<int, Company>();

            var company = db.Query<Company, Employee, Company>(query, (c, e) =>
            {
                if(!companyDic.TryGetValue(c.CompanyId,out var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(currentCompany.CompanyId,currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;
            },splitOn:"EmployeeId");

            return company.Distinct().ToList(); 
        }

        public Company GetCompanyWithEmployees(int id)
        {
            var parameters = new
            {
                CompanyId = id
            };
            var query = "Select * From Companies Where CompanyId = @CompanyId"
                + "Select * From Employees Where CompanyId = @CompanyId";

            Company company;

            using(var lists  = db.QueryMultiple(query, parameters))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        public List<Employee> GetEmployeesWithCompany(int id)
        {
            //var query = "Select * From Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";// SQL Statement
            var query = "Select E.* ,C.* From Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";
            if (id != 0)
            {
                query += "Where E.CompanyId = @Id";
            }
            var employee = db.Query<Employee, Company, Employee>(sql: query, (e, c) =>
            {
                e.Company = c; // insert company into first employee
                return e; // return output(third employee)
            }, new { id }, splitOn: "CompanyId");//Last Employee is Output

            return employee.ToList();
        }
    }
}
