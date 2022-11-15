using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class AchievementController : Controller
    {
        ApplicationContext _context;

        public AchievementController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Achievements.Include(x => x.Pictures).ToListAsync());

        public async Task<IActionResult> ShowAchievement(int achievementId) => View(await _context.Achievements.FindAsync(achievementId));
    }
}
