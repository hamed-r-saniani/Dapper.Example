using Dapper.Contrib.Extensions;
using Dapper.Example.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.Example.Repository
{
    public class CompanyRepositoryDapperContrib : ICompanyRepository
    {
        private IDbConnection db;
        public CompanyRepositoryDapperContrib(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DapperExample"));
        }
        public Company Add(Company company)
        {
            var id = db.Insert(company);
            company.CompanyId = (int)id;
            return company;
        }
        public void Delete(int id)
        {
            db.Delete(new Company() { CompanyId = id});
        }

        public Company Find(int id)
        {
            return db.Get<Company>(id);
        }

        public List<Company> GetAll()
        {
            return db.GetAll<Company>().ToList();
        }

        public bool IsExists(int id)
        {
            var company = Find(id);
            return (company == null);
        }

        public Company Update(Company company)
        {
            db.Update(company);
            return company;
        }
    }
}
