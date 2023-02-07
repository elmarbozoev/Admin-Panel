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

        public async Task<IActionResult> Index()
        {
            var collection = new ModelsCollection();
            collection.Teachers = await _context.Teachers.Include(x => x.ProfilePicture).ToListAsync();
            collection.News = await _context.News.Include(x => x.MediaFiles).ToListAsync();
            collection.Achievements = await _context.Achievements.Include(x => x.MediaFiles).ToListAsync();

            return View(collection);
        }

        [Authorize]
        public IActionResult AdminIndex()
        {
            return View();
        }

        public IActionResult LessonTuple()
        {
            var news = _context.News.ToList();
            var teachers = _context.Teachers.ToList();
            var model = new Tuple<List<News>, List<Teacher>>(news, teachers);

            return View(model);
        }
        
        public IActionResult LessonClass()
        {
            var coll = new ModelsCollection();
            coll.News = _context.News.ToList();
            coll.Teachers = _context.Teachers.ToList();

            return View(coll);
        }

        [HttpPost]
        public IActionResult Search(string query)
        {
            string controller = query.Split('/')[0];
            string action = query.Split('/')[1];
            return RedirectToAction(action, controller);
        }

    }
}