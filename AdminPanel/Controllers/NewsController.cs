﻿using AdminPanel.Interfaces;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdminPanel.Controllers
{
    public class NewsController : Controller, ICRUDController
    {
        ApplicationContext _context;
        IWebHostEnvironment _environment;

        public NewsController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index() => View(await _context.News.Include(x => x.MediaFiles).ToListAsync());

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id) => View(await _context.News.Include(x => x.MediaFiles).FirstOrDefaultAsync(x => x.Id == id));

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int id) => View(await _context.News.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Update(int id, string name, string description)
        {
            var news = await _context.News.FindAsync(id);
            news.Name = name;
            news.Description = description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _context.News.FindAsync(id);
            foreach(var mediaFile in news.MediaFiles)
            {
                System.IO.File.Delete(_environment.WebRootPath + mediaFile.Path);
            }
            _context.MediaFiles.RemoveRange(news.MediaFiles);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, FormFileCollection uploadedFiles, string dateOfPublication)
        {
            var news = new News() { Name = name, Description = description, DateOfPublication = dateOfPublication };
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
            if (uploadedFiles != null)
            {
                foreach(var uploadedFile in uploadedFiles)
                {
                    var path = "/Media/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    var mediaFile = new MediaFile() { Path = path };
                    await _context.MediaFiles.AddAsync(mediaFile);
                    await _context.SaveChangesAsync();
                    news.MediaFiles.Add(mediaFile);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateMedia(int id) => View(await _context.News.Include(x => x.MediaFiles).FirstOrDefaultAsync(x => x.Id == id));

        [HttpPost]
        public async Task<IActionResult> MakeMainMediaFile(int id, int mediaFileId)
        {
            var mediaFile = await _context.MediaFiles.FindAsync(mediaFileId);
            var news = await _context.News.FindAsync(id);
            news.MediaFiles.Remove(mediaFile);
            news.MediaFiles.Insert(0, mediaFile);
            await _context.SaveChangesAsync();
            return RedirectToAction("UpdateMedia", new {id = id});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMediaFile(int id, string mediaFileId)
        {
            var mediaFile = await _context.MediaFiles.FindAsync(mediaFileId);
            System.IO.File.Delete(_environment.WebRootPath + mediaFile.Path);
            _context.MediaFiles.Remove(mediaFile);
            await _context.SaveChangesAsync();
            return RedirectToAction("UpdateMedia", new {id = id});
        }

        [HttpPost]
        public async Task<IActionResult> AddMediaFiles(int id, FormFileCollection uploadedFiles)
        {
            var news = await _context.News.FindAsync(id);
            if (uploadedFiles!=null)
            {
                foreach(var uploadedFile in uploadedFiles)
                {
                    var path = "/Media/" + uploadedFile.FileName;
                    using(var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    var mediaFile = new MediaFile() { Path = path };
                    await _context.MediaFiles.AddAsync(mediaFile);
                    await _context.SaveChangesAsync();
                    news.MediaFiles.Add(mediaFile);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("UpdateMedia", new { id = id });
        }
    }
}
