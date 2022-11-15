using AdminPanel.Interfaces;

namespace AdminPanel.Models
{
    public class Achievement : IContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Picture>? Pictures { get; set; }

        public Achievement()
        {
            Pictures = new List<Picture>();
        }
    }
}
