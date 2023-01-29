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

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string query)
        {
            string contrroller = query.Split('/')[0];
            string action = query.Split('/')[1];
            return RedirectToAction(action, contrroller);
        }

    }
}