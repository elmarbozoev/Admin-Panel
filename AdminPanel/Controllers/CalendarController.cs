using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
