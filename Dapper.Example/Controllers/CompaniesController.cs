using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dapper.Example.Models;
using Dapper.Example.Repository;
using System.Data;

namespace Dapper.Example.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _context;
        private readonly IBonusRepository _bonusRepository;
        private readonly IDapperGenericRepository _dapperGenericRepository;

        public CompaniesController(ICompanyRepository context, IBonusRepository bonusRepository, IDapperGenericRepository dapperGenericRepository)
        {
            _context = context;
            _bonusRepository = bonusRepository;
            _dapperGenericRepository = dapperGenericRepository;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //return View( _context.GetAll());
            return View(_dapperGenericRepository.List<Company>("usp_GetAllCompany"));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company = _bonusRepository.GetCompanyWithEmployees(id.Value);
            var company = _dapperGenericRepository.Single<Company>("usp_GetCompany", new {CompanyId = id.GetValueOrDefault()});
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(company);
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Name", company.Name);
                parameters.Add("@Address", company.Address);
                parameters.Add("@City", company.City);
                parameters.Add("@State", company.State);
                parameters.Add("@PostalCode", company.PostalCode);
                _dapperGenericRepository.Execute("usp_AddCompany", parameters);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _context.Find(id.Value);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _context.Find(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _context.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.IsExists(id);
        }
    }
}
