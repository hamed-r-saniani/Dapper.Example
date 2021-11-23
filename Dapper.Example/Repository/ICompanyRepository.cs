using Dapper.Example.Models;
using System.Collections.Generic;

namespace Dapper.Example.Repository
{
    public interface ICompanyRepository
    {
        Company Find(int id);
        List<Company> GetAll();
        Company Add(Company company);
        Company Update(Company company);
        void Delete(int id);
        bool IsExists(int id);
    }
}
