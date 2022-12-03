namespace AdminPanel.Models
{
    public enum EventType
    {
        Default, //серый
        Weekend, // красный
        Holyday,// зеленый
        SchoolEvent, // оранжевый
        PublicHolyday, // желтый,
        StudyEvent // коричневый
    }

    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Day { get; set; }
        public int DayOfWeek { get; set; }
        public int MonthId { get; set; }
        public Month? Month { get; set; }
        public EventType? Type { get; set; }

        public Event()
        {
            Name = "Regular Day";
            Description = "No Description";
        }
    }
}
