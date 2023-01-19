using AdminPanel.Models;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var eventList = await _context.Calendars.Include(x => x.Events).FirstOrDefaultAsync(x => x.Month == (DateTime.Now.Month <= 10 ? '0' + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) && x.Year == DateTime.Now.Year.ToString());
            return View(eventList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string eventDate, string name, string description)
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

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int eventId, string name)
        {
            var @event = await _context.Events.FindAsync(eventId);
            @event.Name = name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            int? calendarId = @event?.CalendarId;
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            var calendar = await _context.Calendars.Include(x => x.Events).FirstOrDefaultAsync(x => x.Id == calendarId);
            if (calendar.Events.Count == 0)
            {
                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
