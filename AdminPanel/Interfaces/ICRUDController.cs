using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Interfaces
{
    public interface ICRUDController
    {
        Task<IActionResult> Index();
        IActionResult Create();
        Task<IActionResult> Details(int id);
        Task<IActionResult> Update(int id);
        Task<IActionResult> Delete(int id);

        Task<IActionResult> UpdateMedia(int id);
    }
}
