using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Interfaces
{
    public interface IContent
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }    
        string Media { get; set; }
        Task<IActionResult> Create();
        Task<IActionResult> Edit();
        Task<IActionResult> Delete();


    }
}
