using Dapper.Example.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.Example.Repository
{
    public interface IBonusRepository 
    {
        List<Employee> GetEmployeesWithCompany();
    }

    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;
        public BonusRepository(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DapperExample"));
        }
        public List<Employee> GetEmployeesWithCompany()
        {
            //var query = "Select * From Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";// SQL Statement
            var query = "Select E.* ,C.* From Employees AS E INNER JOIN Companies AS C ON E.CompanyId = C.CompanyId";
            var employee = db.Query<Employee, Company, Employee>(sql: query, (e, c) =>
            {
                e.Company = c; // insert company into first employee
                return e; // return output(third employee)
            },splitOn:"CompanyId");//Last Employee is Output

            return employee.ToList();
        }
    }
}
