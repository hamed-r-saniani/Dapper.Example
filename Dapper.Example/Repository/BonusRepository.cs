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
