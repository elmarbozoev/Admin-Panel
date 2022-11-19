using AdminPanel.Interfaces;
using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class AchievementController : Controller, ICRUDController
    {
        ApplicationContext _context;
        IWebHostEnvironment _environment;

        public AchievementController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Create()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Details(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Update(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateMedia(int id)
        {
            throw new NotImplementedException();
        }
    }
}
