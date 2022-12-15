namespace AdminPanel.Models
{
    public class CalendarItem
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string? DayOfWeek { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public ICollection<Event>? Events { get; set; }

        public CalendarItem()
        {
            Events = new List<Event>();
        }
    }
}
