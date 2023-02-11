using AdminPanel.Interfaces;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task<IActionResult> Index() => View(await _context.Achievements.Include(x => x.MediaFiles).ToListAsync());

        [Authorize]
        public async Task<IActionResult> Details(int id) => View(await _context.Achievements.Include(x => x.MediaFiles).FirstOrDefaultAsync(x => x.Id == id));

        [Authorize]
        public async Task<IActionResult> Update(int id) => View(await _context.Achievements.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Update(int id, string name, string description)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            achievement.Name = name;
            achievement.Description = description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var achievement = await _context.Achievements.Include(x => x.MediaFiles).FirstOrDefaultAsync(x => x.Id == id);
            foreach(var mediaFile in achievement.MediaFiles)
            {
                System.IO.File.Delete(_environment.WebRootPath + mediaFile.Path);
            }
            _context.MediaFiles.RemoveRange(achievement.MediaFiles);
            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, FormFileCollection uploadedFiles, string dateOfPublication)
        {
            var achievement = new Achievement() { Name = name, Description = description, DateOfPublication = dateOfPublication };
            await _context.Achievements.AddAsync(achievement);
            await _context.SaveChangesAsync();
            if (uploadedFiles != null)
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    var path = "/Media/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    var mediaFile = new MediaFile() { Path = path };
                    await _context.MediaFiles.AddAsync(mediaFile);
                    await _context.SaveChangesAsync();
                    achievement.MediaFiles.Add(mediaFile);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> UpdateMedia(int id) => View(await _context.Achievements.Include(x => x.MediaFiles).FirstOrDefaultAsync(x => x.Id == id));

        [HttpPost]
        public async Task<IActionResult> DeleteMediaFile(int id, int mediaFileId)
        {
            var mediaFile = await _context.MediaFiles.FindAsync(mediaFileId);
            System.IO.File.Delete(_environment.WebRootPath + mediaFile.Path);
            _context.MediaFiles.Remove(mediaFile);
            await _context.SaveChangesAsync();
            return RedirectToAction("UpdateMedia", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddMediaFiles(int id, FormFileCollection uploadedFiles)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (uploadedFiles != null)
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    var path = "/Media/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    var mediaFile = new MediaFile() { Path = path };
                    await _context.MediaFiles.AddAsync(mediaFile);
                    await _context.SaveChangesAsync();
                    achievement.MediaFiles.Add(mediaFile);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("UpdateMedia", new { id = id });
        }
    }
}
