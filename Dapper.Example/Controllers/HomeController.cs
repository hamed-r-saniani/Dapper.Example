using Dapper.Example.Models;
using Dapper.Example.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonusRepository;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonusRepository)
        {
            _logger = logger;
            _bonusRepository = bonusRepository;
        }

        public IActionResult Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            { 
                return View(_bonusRepository.GetCompanyBy(search));
            }
            return View(_bonusRepository.GetAllCompanyWithEmployees());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddRecords()
        {
            Company company = new Company()
            {
                Name = "Test" + Guid.NewGuid().ToString(),
                Address = "Test Address",
                City = "Test City",
                PostalCode = "Test PostalCode",
                State = "Test State",
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Name = "Test" + Guid.NewGuid().ToString(),
                Email = "Test Email",
                Phone = "Test Phone",
                Title = "Test Title"
            });
            company.Employees.Add(new Employee()
            {
                Name = "Test" + Guid.NewGuid().ToString(),
                Email = "Test Email 2",
                Phone = "Test Phone 2",
                Title = "Test Title 2"
            });
            _bonusRepository.AddTestRecordsWithTransaction(company);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveRecords(int id)
        {
            _bonusRepository.RemoveTestRecords(id);
            return RedirectToAction("Index");
        }
    }
}
