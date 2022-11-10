using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllEmployees() => View(_context.Employees.Include(x => x.Company).ToList());

        [HttpPost]
        public async Task<IActionResult> Create(string employeeName, string companyName)
        {
            // экземпляр класса компании
            Company company = new Company() { Name=companyName };
            // добавляем в базу через контекст
            await _context.AddAsync(company);
            // сохраняем изменения
            await _context.SaveChangesAsync();

            Employee employee = new Employee() { Name = employeeName, Company = company };
            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllEmployees");
        }
    }
}