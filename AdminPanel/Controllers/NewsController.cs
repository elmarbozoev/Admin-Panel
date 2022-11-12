using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdminPanel.Controllers
{
    public class NewsController : Controller
    {
        ApplicationContext _context;
        IWebHostEnvironment _environment;

        public NewsController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> ShowNews(int newsId) => View(await _context.News.Include(x => x.Pictures).FirstOrDefaultAsync(x => x.Id == newsId));

        public async Task<IActionResult> Index() => View(await _context.News.Include(x => x.Pictures).ToListAsync());

        [HttpGet]
        public async Task<IActionResult> EditNews(int id) => View(await _context.News.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> EditNews(int newsId, string newsName, string newsDescription)
        {
            var news = await _context.News.FindAsync(newsId);
            news.Name = newsName;
            news.Description = newsDescription;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(int newsId)
        {
            var news = await _context.News.FindAsync(newsId);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateNews() => View();

        [HttpPost]
        public async Task<IActionResult> CreateNews(string newsName, string newsDescription, IFormFileCollection uploadedFiles, DateTime dateOfPublication)
        {
            var news = new News() { Name = newsName, Description = newsDescription, DateOfPublication = dateOfPublication };
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();

            if (uploadedFiles != null)
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    var path = "/Pictures/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    await _context.Pictures.AddAsync(new Pictures() { Name = uploadedFile.FileName, Path = path, News = news });
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditPictures(int id)
        {
            var news = await _context.News.Include(x => x.Pictures).FirstOrDefaultAsync(x => x.Id == id);
            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> MakeMainPicture(int newsId, int pictureIndex)
        {
            var news = await _context.News.FindAsync(newsId);
            news.MainPictureIndex = pictureIndex;
            await _context.SaveChangesAsync();
            return RedirectToAction("EditPictures", new { id = newsId });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            var picture = await _context.Pictures.FindAsync(pictureId);
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditPictures", new { id = picture.NewsId });
        }
        //test
        [HttpPost]
        public async Task<IActionResult> AddPictures(int newsId, IFormFileCollection uploadedFiles)
        {
            var news = await _context.News.FindAsync(newsId);
            if(uploadedFiles != null)
            {
                foreach(var uploadedFile in uploadedFiles)
                {
                    var path = "/Pictures/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    await _context.Pictures.AddAsync(new Pictures() { Name = uploadedFile.Name, Path = path, News = news });
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("EditPictures", new { id = newsId });
        }
    }
}
