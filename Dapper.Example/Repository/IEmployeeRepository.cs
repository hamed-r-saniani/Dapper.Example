using Dapper.Example.Models;
using System.Collections.Generic;

namespace Dapper.Example.Repository
{
    public interface IEmployeeRepository
    {
        Employee Find(int id);
        List<Employee> GetAll();
        Employee Add(Employee company);
        Employee Update(Employee company);
        void Delete(int id);
        bool IsExists(int id);
    }
}
