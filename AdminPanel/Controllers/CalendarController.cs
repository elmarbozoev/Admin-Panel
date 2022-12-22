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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(string dateEvent, string name, string description)
        {
            var day = "";
            var month = "";
            var year = "";
            foreach(var i in dateEvent.Split())
            {
                year = i.Split('-')[2];
                month = i.Split('-')[1];
                day = i.Split("-")[0];
            }
            Calendar calendar = new Calendar() { Month = month, Year = year };
            await _context.Calendars.AddAsync(calendar);
            await _context.SaveChangesAsync();

            Event @event = new Event() { Day = day, Name = name, Description = description, Calendar = calendar };
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();

            return View("Index");
        }
    }
}
