using Dapper.Example.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dapper.Example.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Company Add(Company company)
        {
            _context.Companies.Add(company);
            Save();
            return company;
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var company = Find(id);
            _context.Companies.Remove(company);
            Save();
        }

        public Company Find(int id)
        {
            return _context.Companies.Find(id);
        }

        public List<Company> GetAll()
        {
            return _context.Companies.ToList();
        }

        public Company Update(Company company)
        {
            _context.Companies.Update(company);
            Save();
            return company;
        }

        public bool IsExists(int id)
        {
            return _context.Companies.Any(x => x.CompanyId == id);
        }
    }
}
