using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Interfaces
{
    public interface IContent  
    {
        //рекомендация
        // так как в "достижениях" тоже будет фото/видео, то проще класс Pictures переименовать в AffFiles/Media или еще как-нибудь
        //и так как CRUD везде одинаковые,нужно создать еще один интерфейс с операоцяими CRUD, включая CreateFile и Detail
        //Каждый контролллер имплементировать от интерфейса
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }    
        string Media { get; set; }


        //это зачем тут? новый интерфейс нужен. Выше описала
        Task<IActionResult> Create();
        Task<IActionResult> Edit();
        Task<IActionResult> Delete();


    }
}
