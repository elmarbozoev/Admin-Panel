using AdminPanel.Interfaces;
using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class AchievementController : Controller, ICRUDController
    {
        ApplicationContext _context;

        public AchievementController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Delete(int contentId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Details(int contentId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Update(int contentId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateMedia(int id)
        {
            throw new NotImplementedException();
        }
    }
}
