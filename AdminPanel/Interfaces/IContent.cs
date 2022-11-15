using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Interfaces
{
    public interface IContent 
    {
        int Id { get; set; }
<<<<<<< Updated upstream
        string Name { get; set; }
        string Description { get; set; }    
        string Media { get; set; }
        Task<IActionResult> Create();
        Task<IActionResult> Edit();
        Task<IActionResult> Delete();


=======
        string? Name { get; set; }
        string? Description { get; set; }    
        ICollection<Picture>? Pictures { get; set; }
>>>>>>> Stashed changes
    }
}
