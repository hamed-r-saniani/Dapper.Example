using Dapper.Example.Models;
using Dapper.Example.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.Example.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IBonusRepository _bonusRepository;

        public EmployeesController(IEmployeeRepository context, ICompanyRepository companyRepository, IBonusRepository bonusRepository)
        {
            _employeeRepository = context;
            _companyRepository = companyRepository;
            _bonusRepository = bonusRepository;
        }
        [BindProperty]
        public Employee Employee { get; set; }

        // GET: Employees
        public async Task<IActionResult> Index(int id = 0)
        {
            //List<Employee> employees = _employeeRepository.GetAll();
            //foreach (Employee item in employees)
            //{
            //    item.Company = _companyRepository.Find(item.CompanyId);
            //}

            return View(_bonusRepository.GetEmployeesWithCompany(id));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _employeeRepository.Find(id.Value);
            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "Name");
            return View(Employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _employeeRepository.Find(id.Value);
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "Name", Employee.CompanyId);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(Employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(Employee.EmployeeId))
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
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "Name");
            return View(Employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _employeeRepository.Find(id.Value);
            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _employeeRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _employeeRepository.IsExists(id);
        }
    }
}
