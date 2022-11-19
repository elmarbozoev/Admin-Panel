namespace AdminPanel.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NewsId { get; set; }
        public News? News { get; set; }
        public string? Path { get; set; }
    }
}
