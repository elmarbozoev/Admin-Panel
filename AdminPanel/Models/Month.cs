namespace AdminPanel.Models
{
    public class Month
    {
        public int Id { get; set; }
        public int NumberOfMonth { get; set; }
        public int Year { get; set; }
        public ICollection<Event>? Events { get; set; }

        public Month()
        {
            Events = new List<Event>();
        }
    }
}
