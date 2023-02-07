using AdminPanel.Interfaces;

namespace AdminPanel.Models
{
    public class News : IContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<MediaFile>? MediaFiles { get; set; }
        public string? DateOfPublication { get; set; }

        public News()
        {
            MediaFiles = new List<MediaFile>();
        }

    }
}
