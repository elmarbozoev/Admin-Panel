using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AdminPanel.Models
{
    public enum EventType
    {
        SchoolEvent,
        PublicHoliday,
        StudyEvent
    }

    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CalendarItemId { get; set; }
        public EventType? Type { get; set; }
    }
}
