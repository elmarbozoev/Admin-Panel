using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdminPanel.Controllers
{
    public class NewsController : Controller
    {
        // Контекст для связи с БД
        ApplicationContext _context;
        // Среда для связи с wwwroot
        IWebHostEnvironment _environment;

        // В конструкторе выше указанные переменные инициализируются
        public NewsController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Отображает страничку с конкретной новостью
        public async Task<IActionResult> ShowNews(int newsId) => View(await _context.News.Include(x => x.Pictures).FirstOrDefaultAsync(x => x.Id == newsId));

        // Отображает страничку со всеми новостями
        public async Task<IActionResult> Index() => View(await _context.News.Include(x => x.Pictures).ToListAsync());

        // Отображает страничку для редактирования конкретной новости (используется просто id, т.к. newsId он почему-то видеть не хочет
        [HttpGet]
        public async Task<IActionResult> EditNews(int id) => View(await _context.News.FindAsync(id));

        // Получает данные со странички для редактирования новости и обрабатывает их
        [HttpPost]
        public async Task<IActionResult> EditNews(int newsId, string newsName, string newsDescription)
        {
            // Находим редактируемую новость с помощью ее id (newsId)
            var news = await _context.News.FindAsync(newsId);
            // Присваиваем новое наименование новости
            news.Name = newsName;
            // Присваиваем новое описание новости
            news.Description = newsDescription;
            // Сохраняем изменения в БД
            await _context.SaveChangesAsync();
            // Перенаправляем в Index
            return RedirectToAction("Index");
        }

        // Удаляет конкретную новость
        [HttpPost]
        public async Task<IActionResult> DeleteNews(int newsId)
        {
            // Находим новость по ее id
            var news = await _context.News.FindAsync(newsId);
            // Удаляем новость
            _context.News.Remove(news);
            // Сохраняем изменения в БД
            await _context.SaveChangesAsync();
            // Перенаправляем в Index
            return RedirectToAction("Index");
        }

        // Отображает страничку для создания новости
        [HttpGet]
        public IActionResult CreateNews() => View();

        // Получает данные со странички для создания новости и обрабатывает их
        [HttpPost]
        public async Task<IActionResult> CreateNews(string newsName, string newsDescription, IFormFileCollection uploadedFiles)
        {
            // Создаем экземпляр класса News, т.е нашу новость, даем ем наименование и описание
            var news = new News() { Name = newsName, Description = newsDescription };
            // Добавляем ее в БД через контекст
            await _context.News.AddAsync(news);
            // Сохраняем измененения
            await _context.SaveChangesAsync();

            // Проверка на то, что загружаемые файлы, т.е картинки не пустые
            if (uploadedFiles != null)
            {
                // Пробежка по всем картинкам
                foreach (var uploadedFile in uploadedFiles)
                {
                    // Создания пути, куда загрузится картинка
                    var path = "/Pictures/" + uploadedFile.FileName;
                    // Загружаем картинку по нужному пути, используем using, т.к. код не управляемый
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    // Создаем для картинки экземпляр класса Pictures, который мы загрузим в БД
                    await _context.Pictures.AddAsync(new Pictures() { Name = uploadedFile.FileName, Path = path, News = news });
                    // Сохраняем изменения
                    await _context.SaveChangesAsync();
                }
            }
            // Перенаправляем в Index
            return RedirectToAction("Index");
        }

        // Отображает страничку со всеми картинками новости для их редактирования (здесь тоже почему-то мистическим образом ничего кроме id не воспринимает :))
        [HttpGet]
        public async Task<IActionResult> EditPictures(int id)
        {
            // Находим новость по ее id
            var news = await _context.News.Include(x => x.Pictures).FirstOrDefaultAsync(x => x.Id == id);
            // Отправляем ее картинки во View
            return View(news?.Pictures?.ToList());
        }

        // Делает картинку главной
        [HttpPost]
        public async Task<IActionResult> MakeMainPicture(int newsId, int pictureIndex)
        {
            // Находим новость по айди
            var news = await _context.News.FindAsync(newsId);
            // Устанавливаем индекс главной картинки в ICollection<Pictures>
            news.MainPictureIndex = pictureIndex;
            // Сохраняем изменения в БД
            await _context.SaveChangesAsync();
            // Перенаправляем в EditPictures
            return RedirectToAction("EditPictures", new { id = newsId });
        }

        // Удаляет выбранную картинку
        [HttpPost]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            // Находим картинку по ее айди в БД
            var picture = await _context.Pictures.FindAsync(pictureId);
            // Удаляем картинку
            _context.Pictures.Remove(picture);
            //Сохраняем изменения в БД
            await _context.SaveChangesAsync();
            // Перенаправляем в EditPictures
            return RedirectToAction("EditPictures", new { id = picture.NewsId });
        }

        // Добавляет картинки к новости
        [HttpPost]
        public async Task<IActionResult> AddPictures(int newsId, IFormFileCollection uploadedFiles)
        {
            // Находим новость по ее айди
            var news = await _context.News.FindAsync(newsId);
            // Проверяем, что картинки правда есть
            if(uploadedFiles != null)
            {
                // Пробегаем по всем картинкам
                foreach(var uploadedFile in uploadedFiles)
                {
                    // Создаем путь, куда загрузим картинку
                    var path = "/Pictures/" + uploadedFile.FileName;
                    // Загружаем картинку
                    using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    // Добавляем картинку в БД
                    await _context.Pictures.AddAsync(new Pictures() { Name = uploadedFile.Name, Path = path, News = news });
                    // Сохраняем изменения в БД
                    await _context.SaveChangesAsync();
                }
            }
            // Перенаправляем в EditPictures
            return RedirectToAction("EditPictures", new { id = newsId });
        }
    }
}
