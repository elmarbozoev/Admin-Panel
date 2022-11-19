using AdminPanel.Interfaces;

namespace AdminPanel.Models
{
    public class Achievement : IContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<MediaFile>? MediaFiles { get; set; }

        public Achievement()
        {
            MediaFiles = new List<MediaFile>();
        }
    }
}
