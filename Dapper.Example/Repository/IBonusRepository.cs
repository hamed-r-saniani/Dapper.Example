using Dapper.Example.Models;
using System.Collections.Generic;

namespace Dapper.Example.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeesWithCompany(int id);
        Company GetCompanyWithEmployees(int id);
        List<Company> GetAllCompanyWithEmployees();
        void AddTestRecords(Company command);
        void RemoveTestRecords(int id);
        List<Company> GetCompanyBy(string name);
    }
}
