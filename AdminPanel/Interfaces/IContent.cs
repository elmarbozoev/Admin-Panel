using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Interfaces
{
    public interface IContent 
    {
        int Id { get; set; }
        string? Name { get; set; }
        string? Description { get; set; }    
        ICollection<Picture>? Pictures { get; set; }
    }
}
