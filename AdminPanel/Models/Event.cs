using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AdminPanel.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Day { get; set; }
        public int? CalendarId { get; set; }
        public Calendar? Calendar { get; set; }
    }
}
