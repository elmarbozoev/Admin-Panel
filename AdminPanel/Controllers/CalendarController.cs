using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class CalendarController : Controller
    {
        ApplicationContext _context;
        Dictionary<int, string> _months;

        public CalendarController(ApplicationContext context)
        {
            _context = context;
            _months = new Dictionary<int, string>()
            {
                {1, "January" },
                {2, "February" },
                {3, "March" },
                {4, "April" },
                {5, "May" },
                {6, "June" },
                {7, "July" },
                {8, "August" },
                {9, "September" },
                {10, "October" },
                {11, "November" },
                {12, "December" }
            };
        }

        public async Task<IActionResult> Index()
        {
            var month = await _context.Months.Include(x => x.Events).LastOrDefaultAsync(x => x.NumberOfMonth == DateTime.Now.Month && x.Year == DateTime.Now.Year);
            if (month == null)
            {
                month = new Month() { NumberOfMonth = DateTime.Now.Month, Year = DateTime.Now.Year };
                await _context.Months.AddAsync(month);
                await _context.SaveChangesAsync();
                for(int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                {
                    var monthEvent = new Event() { Day = i+1, DayOfWeek = (int)DateTime.Now.DayOfWeek, Month = month, MonthId = month.Id, Type = EventType.Default };
                    await _context.Events.AddAsync(monthEvent);
                    await _context.SaveChangesAsync();
                    month.Events.Add(monthEvent);
                    await _context.SaveChangesAsync();
                }
            }
            return View(month);
        }
    }
}
