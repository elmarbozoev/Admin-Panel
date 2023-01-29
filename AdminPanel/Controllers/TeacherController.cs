using AdminPanel.Interfaces;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System.IO;

namespace AdminPanel.Controllers
{
    public class TeacherController : Controller, ICRUDController
    {
        ApplicationContext _context;
        IWebHostEnvironment _environment;

        public TeacherController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _context.Teachers.Include(x => x.ProfilePicture).ToListAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name, string subject, IFormFile profilePicture)
        {
            var teacher = new Teacher() { Name = name, Subject = subject };
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();

            if (profilePicture is not null)
            {
                var path = "/Media/" + profilePicture.FileName;
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }
                var pfp = new MediaFile() { Name = profilePicture.FileName, Path = path };
                await _context.MediaFiles.AddAsync(pfp);
                await _context.SaveChangesAsync();
                teacher.ProfilePicture = await _context.MediaFiles.FindAsync(pfp.Id);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers.Include(x => x.ProfilePicture).FirstOrDefaultAsync(x => x.Id == id);
            System.IO.File.Delete(_environment.WebRootPath + teacher.ProfilePicture.Path);
            _context.MediaFiles.Remove(teacher.ProfilePicture);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id) => View(await _context.Teachers.Include(x => x.ProfilePicture).FirstOrDefaultAsync(x => x.Id == id));

        [HttpGet]
        public async Task<IActionResult> Update(int id) => View(await _context.Teachers.Include(x => x.ProfilePicture).FirstOrDefaultAsync(x => x.Id == id));

        [HttpPost]
        public async Task<IActionResult> Update(int id, string name, string subject, IFormFile profilePicture)
        {
            var teacher = await _context.Teachers.Include(x => x.ProfilePicture).FirstOrDefaultAsync(x => x.Id == id);
            teacher.Name = name;
            teacher.Subject = subject;
            await _context.SaveChangesAsync();

            if (profilePicture is not null)
            {
                System.IO.File.Delete(_environment.WebRootPath + teacher.ProfilePicture.Path);
                var path = "/Media/" + profilePicture.FileName;
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }
                teacher.ProfilePicture.Name = profilePicture.FileName;
                teacher.ProfilePicture.Path = path;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public Task<IActionResult> UpdateMedia(int id)
        {
            throw new NotImplementedException();
        }
    }
}
