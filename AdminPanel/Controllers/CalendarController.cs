using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class CalendarController : Controller
    {
        ApplicationContext _context;

        public CalendarController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var days = await _context.CalendarItems.Include(x => x.Events).Where(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).ToListAsync();
            return View(days);
        }



    }
}
