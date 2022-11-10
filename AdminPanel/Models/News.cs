namespace AdminPanel.Models
{
    public class News
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Pictures>? Pictures { get; set; }
        public int MainPictureIndex { get; set; }

        public DateTime DateOfPublication { get; set; }

        public News()
        {
            Pictures = new List<Pictures>();
        }

    }
}
