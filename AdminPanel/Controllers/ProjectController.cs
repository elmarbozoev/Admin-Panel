using AdminPanel.Interfaces;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class ProjectController : Controller, ICRUDController
    {
        ApplicationContext _context;
        IWebHostEnvironment _environment;

        public ProjectController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index() => View(await _context.Projects.Include(x => x.Preview).ToListAsync());

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name, IFormFile preview)
        {
            var project = new Project() { Name = name };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            if (preview is not null)
            {
                var path = "/Media/" + preview.FileName;
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await preview.CopyToAsync(fileStream);
                }
                var mediaFile = new MediaFile() { Path = path };
                await _context.MediaFiles.AddAsync(mediaFile);
                await _context.SaveChangesAsync();
                project.Preview = mediaFile;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.Include(x => x.Preview).FirstOrDefaultAsync(x => x.Id == id);
            System.IO.File.Delete(_environment.WebRootPath + project.Preview.Path);
            _context.MediaFiles.Remove(project.Preview);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int id) => View(await _context.Projects.Include(x => x.Preview).FirstOrDefaultAsync(x => x.Id == id));

        [HttpPost]
        public async Task<IActionResult> Update(int id, string name, IFormFile preview)
        {
            var project = await _context.Projects.Include(x => x.Preview).FirstOrDefaultAsync(x => x.Id == id);
            project.Name = name;
            await _context.SaveChangesAsync();

            if (preview is not null)
            {
                System.IO.File.Delete(_environment.WebRootPath + project.Preview);
                var path = "/Media/" + preview.FileName;
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await preview.CopyToAsync(fileStream);
                }
                project.Preview.Path = path;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> UpdateMedia(int id)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> Details(int id)
        {
            throw new NotImplementedException();
        }
    }
}
