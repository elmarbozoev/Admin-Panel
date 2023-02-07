using AdminPanel.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Models
{
    public class Achievement : IContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<MediaFile>? MediaFiles { get; set; }
        public string? DateOfPublication { get; set; }

        public Achievement()
        {
            MediaFiles = new List<MediaFile>();
        }
    }
}
