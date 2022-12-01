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
    }
}