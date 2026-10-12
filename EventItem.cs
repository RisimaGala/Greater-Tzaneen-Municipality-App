using System;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// Urgency ranking for an event or announcement. Lower numeric value means
    /// more urgent; this lets the value be combined with a date offset to build
    /// a single sortable priority key (see EventCatalog.BuildPriorityKey).
    /// </summary>
    public enum EventPriority
    {
        Urgent = 0,
        High = 1,
        Normal = 2,
        Low = 3
    }

    /// <summary>
    /// A single local event or municipal announcement.
    /// </summary>
    public class EventItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string TimeLabel { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public EventPriority Priority { get; set; }

        public override string ToString()
        {
            return Title + " (" + Date.ToString("dd MMM yyyy") + ")";
        }
    }
}
