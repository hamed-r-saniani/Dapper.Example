using Dapper.Example.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.Example.Repository
{
    public class CompanyRepositoryDapper : ICompanyRepository
    {
        private IDbConnection db;
        public CompanyRepositoryDapper(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DapperExample"));
        }
        public Company Add(Company company)
        {
            string query = "Insert Into Companies(Name,Address,City,State,PostalCode) values(@Name,@Address,@City,@State,@PostalCode)" +
                "Select CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(query, new { @Name = company.Name, @Address = company.Address, @City = company.City, @State = company.State, @PostalCode = company.PostalCode }).SingleOrDefault();
            company.CompanyId = id;
            return company;
        }

        public void Delete(int id)
        {
            string query = "Delete From Companies Where CompanyId = @Id";
            db.Execute(query, new { id });
        }

        public Company Find(int id)
        {
            string query = "Select * From Companies Where CompanyId = @CompanyId";
            return db.Query<Company>(query, new { @CompanyId = id }).SingleOrDefault();
        }

        public List<Company> GetAll()
        {
            string query = "Select * From Companies";
            return db.Query<Company>(query).ToList();
        }
        
        public bool IsExists(int id)
        {
            var company = Find(id);
            return (company == null);
        }

        public Company Update(Company company)
        {
            string query = "Update Companies SET Name = @Name,Address = @Address,City = @City,State = @State,PostalCode = @PostalCode Where CompanyId = @CompanyId";
            db.Execute(query, company);
            return company;
        }
    }
}
