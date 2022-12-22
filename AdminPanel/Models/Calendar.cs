namespace AdminPanel.Models
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public ICollection<Event>? Events { get; set; }

        public Calendar()
        {
            Events = new List<Event>();
        }
    }
}
