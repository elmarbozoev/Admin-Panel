using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace AdminPanel.Models
{
    //унаследуем наш ApplicationContext от DbCOntext (через знак " : ")
    public class ApplicationContext: IdentityDbContext<User>
    {
        //в БД появится таблица Users после миграции.
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<News> News { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<Calendar> Calendars { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<MediaFile> MediaFiles { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;

        //созаем конструктор ApplicationContext, чтоб можно было инициализировать объекты класса AppliCationContext в других
        //классах (контроллеры  втом числе)

        //DbContextOptions служит для установки подключения БД через options,
        //в нашем случае по умолчанию нет никакх значений
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }
    }
}
