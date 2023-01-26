using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AdminPanel.Controllers
{
    public class CalendarController : Controller
    {
        ApplicationContext _context;

        public CalendarController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int month, int year)
        {
            if (month == 0 && year == 0)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            var eventList = await _context.Calendars.Include(x => x.Events).FirstOrDefaultAsync(x => x.Month == (month <= 10 ? '0' + month.ToString() : month.ToString()) && x.Year == year.ToString());
            return View(eventList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string eventDate, string name)
        {
            var year = eventDate.Split('-')[0];
            var month = eventDate.Split('-')[1];
            var day = eventDate.Split('-')[2];
            
            Calendar calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Year == year && x.Month == month);
            
            if (calendar is null)
            {
                calendar = new Calendar() { Month = month, Year = year };
                await _context.Calendars.AddAsync(calendar);
                await _context.SaveChangesAsync();
            }

            Event @event = new Event() { Day = day, Name = name, Calendar = calendar };
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { month = int.Parse(month), year = int.Parse(year) });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int eventId, string name)
        {
            var @event = await _context.Events.FindAsync(eventId);
            @event.Name = name;
            int month = int.Parse(@event.Calendar.Month);
            int year = int.Parse(@event.Calendar.Year);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { month = month, year = year });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            int? calendarId = @event?.CalendarId;
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            var calendar = await _context.Calendars.Include(x => x.Events).FirstOrDefaultAsync(x => x.Id == calendarId);
            int month = int.Parse(calendar.Month);
            int year = int.Parse(calendar.Year);
            if (calendar.Events.Count == 0)
            {
                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { month = month, year = year });
        }
    }
}
