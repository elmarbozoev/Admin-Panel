using Microsoft.AspNetCore.Mvc.Formatters;

namespace AdminPanel.Models
{
    public class MediaFile
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}
